using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class TableService
{
    private readonly TableRepository _tableRepository;
    private readonly RestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public TableService(TableRepository tableRepository, RestaurantService restaurantService, IMapper mapper)
    {
        this._tableRepository = tableRepository;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<TableResponse>> ListAllTablesAsync()
    {
        var tables = await _tableRepository.ListAllAsync();
        return _mapper.Map<List<TableResponse>>(tables);
    }
    
    public async Task<TableResponse> CreateTableAsync(TableRequest tableRequest)
    {
        if (tableRequest is null)
        {
            throw new ArgumentNullException(nameof(tableRequest));
        }

        if (tableRequest.RestaurantId == null ||
            tableRequest.TableNumber == null ||
            tableRequest.Capacity == null)
        {
            throw new ArgumentException("All required table fields (RestaurantId, TableNumber, Capacity) must be provided.");
        }

        if (!await this._restaurantService.IsRestaurantExists(tableRequest.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        var tableEntity = _mapper.Map<Table>(tableRequest);
        await _tableRepository.CreateAsync(tableEntity);
        return _mapper.Map<TableResponse>(tableEntity);
    }

    public async Task<bool> DeleteTableByIdAsync(int tableId)
    {
        var existingTable = await _tableRepository.GetByIdAsync(tableId);
        if (existingTable is null)
        {
            throw new NotFoundException($"The table doesn't exist");
        }
        return await _tableRepository.DeleteByIdAsync(tableId);
    }

    public async Task UpdateTableByIdAsync(int tableId, TableRequest updatedTable)
    {
        if (updatedTable is null)
        {
            throw new ArgumentNullException(nameof(updatedTable));
        }
        var existingTable = await _tableRepository.GetByIdAsync(tableId);
        if (existingTable is null)
        {
            throw new NotFoundException($"The table doesn't exist");
        }
        
        if(updatedTable.RestaurantId.HasValue && !await _restaurantService.IsRestaurantExists(updatedTable.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        _mapper.Map(updatedTable, existingTable);
        await _tableRepository.UpdateAsync(existingTable);
    }
}