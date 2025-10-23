using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class ReservationService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly CustomerService _customerService;
    private readonly RestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public ReservationService(UnitOfWork unitOfWork, CustomerService customerService,
        RestaurantService restaurantService, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._customerService = customerService;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<ReservationResponse>> ListAllReservationAsync()
    {
        var reservations = await _unitOfWork.Reservations.ListAllAsync();
        return _mapper.Map<List<ReservationResponse>>(reservations);
    }
    
    public async Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservationRequest)
    {
        if (reservationRequest is null)
            throw new ArgumentNullException(nameof(reservationRequest));

        if (reservationRequest.CustomerId is null ||
            reservationRequest.RestaurantId is null ||
            reservationRequest.PartySize is null)
        {
            throw new ArgumentException("All required fields (RestaurantId, CustomerId, PartySize) must be provided.");
        }

        if (!await _customerService.IsCustomerExists(reservationRequest.CustomerId.Value))
            throw new NotFoundException("The customer doesn't exist");

        if (!await _restaurantService.IsRestaurantExists(reservationRequest.RestaurantId.Value))
            throw new NotFoundException("The restaurant doesn't exist");

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var reservationEntity = _mapper.Map<Reservation>(reservationRequest);
            var requiredTables = await AssignTablesForPartyAsync(
                reservationRequest.RestaurantId.Value,
                reservationRequest.PartySize.Value
            );

            var createdReservation = await _unitOfWork.Reservations.CreateAsync(reservationEntity);
            await LinkTablesToReservationAsync(createdReservation, requiredTables);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<ReservationResponse>(createdReservation);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteReservationByIdAsync(int reservationId)
    {
        var existingReservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
        if (existingReservation is null)
            throw new NotFoundException($"The reservation doesn't exist");

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await ReleaseReservationTablesAsync(reservationId);
            var result = await _unitOfWork.Reservations.DeleteByIdAsync(reservationId);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateReservationByIdAsync(int reservationId, ReservationRequest updatedReservation)
    {
        if (updatedReservation is null)
            throw new ArgumentNullException(nameof(updatedReservation));

        var existingReservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
        if (existingReservation is null)
            throw new NotFoundException($"The reservation doesn't exist");

        if (updatedReservation.CustomerId.HasValue &&
            !await _customerService.IsCustomerExists(updatedReservation.CustomerId.Value))
            throw new NotFoundException("The customer doesn't exist");

        if (updatedReservation.RestaurantId.HasValue &&
            !await _restaurantService.IsRestaurantExists(updatedReservation.RestaurantId.Value))
            throw new NotFoundException("The restaurant doesn't exist");

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updatedReservation, existingReservation);
            if (updatedReservation.RestaurantId.HasValue || updatedReservation.PartySize.HasValue)
            {
                await ReleaseReservationTablesAsync(reservationId);
                var restaurantId = updatedReservation.RestaurantId ?? existingReservation.RestaurantId;
                var partySize = updatedReservation.PartySize ?? existingReservation.PartySize;
                
                var requiredTables = await AssignTablesForPartyAsync(restaurantId, partySize);
                Console.WriteLine(requiredTables.Count);
                await LinkTablesToReservationAsync(existingReservation, requiredTables);
            }
            
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<ReservationResponse>> GetReservationsByCustomerId(int customerId)
    {
        var reservations = await this._unitOfWork.Reservations.GetReservationsByCustomerId(customerId);
        return _mapper.Map<List<ReservationResponse>>(reservations);
    }

    public async Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySize(int partySize)
    {
        return await this._unitOfWork.Reservations.ListCustomersReservationExceedsPartySize(partySize);
    }
    
    public async Task<bool> IsReservationExists(int reservationId)
    {
        return await _unitOfWork.Reservations.GetByIdAsync(reservationId) != null;
    }
    
    private async Task<List<Table>> AssignTablesForPartyAsync(int restaurantId, int partySize)
    {
        var availableTables = await _unitOfWork.Tables.GetAvailableTablesForRestaurant(restaurantId);
        availableTables = availableTables.OrderByDescending(t => t.Capacity).ToList();

        var requiredTables = new List<Table>();
        var remainingSeats = partySize;

        foreach (var table in availableTables)
        {
            requiredTables.Add(table);
            remainingSeats -= table.Capacity;
            if (remainingSeats <= 0)
                break;
        }

        if (requiredTables.Sum(t => t.Capacity) < partySize)
            throw new NoAvailableTablesException("Sorry, there's no available tables right now.");

        return requiredTables;
    }
    
    private async Task ReleaseReservationTablesAsync(int reservationId)
    {
        var reservationTables = await _unitOfWork.ReservationTable.GetByReservationIdAsync(reservationId);
        foreach (var reservationTable in reservationTables)
        {
            var table = await _unitOfWork.Tables.GetByIdAsync(reservationTable.TableId);
            if (table != null)
                table.IsAvailable = true;

            await _unitOfWork.ReservationTable.DeleteAsync(reservationTable);
        }

        await this._unitOfWork.SaveChangesAsync();
    }
    
    private async Task LinkTablesToReservationAsync(Reservation reservation, List<Table> tables)
    {
        foreach (var table in tables)
        {
            var reservationTable = new ReservationTable
            {
                Reservation = reservation,
                ReservationId = reservation.Id,
                Table = table,
                TableId = table.Id,
                AssignedSeats = table.Capacity
            };

            await _unitOfWork.ReservationTable.CreateAsync(reservationTable);
            table.IsAvailable = false;
        }
    }
}