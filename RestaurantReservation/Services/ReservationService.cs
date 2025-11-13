using System.ComponentModel.DataAnnotations;
using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Helpers;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerService _customerService;
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public ReservationService(IUnitOfWork unitOfWork, ICustomerService customerService,
        IRestaurantService restaurantService, IMapper mapper)
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
        ValidateReservationRequest(reservationRequest);
        await _customerService.EnsureCustomerExistsAsync(reservationRequest.CustomerId!.Value);
        await _restaurantService.EnsureRestaurantExistsAsync(reservationRequest.RestaurantId!.Value);
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var reservationEntity = _mapper.Map<Reservation>(reservationRequest);
            var requiredTables = await AssignTablesForPartyAsync(
                reservationRequest.RestaurantId!.Value,
                reservationRequest.PartySize!.Value
            );

            var createdReservation = await _unitOfWork.Reservations.CreateAsync(reservationEntity);
            await LinkTablesToReservationAsync(createdReservation, requiredTables);

            await _unitOfWork.CommitAsync();
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
        await EnsureReservationExistsAsync(reservationId);
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            await ReleaseReservationTablesAsync(reservationId);
            await _unitOfWork.Reservations.DeleteByIdAsync(reservationId);

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return true;
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
        {
            throw new ArgumentNullException(nameof(updatedReservation));
        }
        
        var existingReservation = await EnsureReservationExistsAsync(reservationId);
        if (updatedReservation.CustomerId.HasValue)
        {
            await _customerService.EnsureCustomerExistsAsync(updatedReservation.CustomerId.Value);
        }

        if (updatedReservation.RestaurantId.HasValue)
        {
            await _restaurantService.EnsureRestaurantExistsAsync(updatedReservation.RestaurantId.Value);
        }
  
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
                await LinkTablesToReservationAsync(existingReservation, requiredTables);
            }
            
            await _unitOfWork.CommitAsync();
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
        var reservations = await this._unitOfWork.Reservations.GetReservationsByCustomerIdAsync(customerId);
        return _mapper.Map<List<ReservationResponse>>(reservations);
    }

    public async Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySize(int partySize)
    {
        return await this._unitOfWork.Reservations.ListCustomersReservationExceedsPartySizeAsync(partySize);
    }
    
    public async Task<Reservation> EnsureReservationExistsAsync(int reservationId)
    {
        var existingReservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
        if (existingReservation is null)
            throw new NotFoundException($"The reservation doesn't exist");

        return existingReservation;
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

        await this._unitOfWork.CommitAsync();
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
    
    private static void ValidateReservationRequest(ReservationRequest reservationRequest)
    {
        ValidationHelper.EnsureNotNull(reservationRequest, nameof(reservationRequest));
        ValidationHelper.EnsureRequiredFields(
            (reservationRequest.CustomerId, nameof(reservationRequest.CustomerId))!,
            (reservationRequest.PartySize, nameof(reservationRequest.PartySize))!,
            (reservationRequest.RestaurantId, nameof(reservationRequest.RestaurantId))!
            );
    }
}