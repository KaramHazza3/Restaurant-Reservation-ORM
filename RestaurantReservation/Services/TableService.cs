using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Intf;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Helpers;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services;

public class TableService : ITableService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public TableService(IUnitOfWork unitOfWork, IRestaurantService restaurantService, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<TableResponse>> ListAllTablesAsync()
    {
        var tables = await _unitOfWork.Tables.ListAllAsync();
        return _mapper.Map<List<TableResponse>>(tables);
    }
    
    public async Task<TableResponse> CreateTableAsync(TableRequest tableRequest)
    {
        ValidateTableRequest(tableRequest);
        await _restaurantService.EnsureRestaurantExistsAsync(tableRequest.RestaurantId!.Value);
        var tableEntity = _mapper.Map<Table>(tableRequest);
        await _unitOfWork.Tables.CreateAsync(tableEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<TableResponse>(tableEntity);
    }

    public async Task<bool> DeleteTableByIdAsync(int tableId)
    {
        await EnsureTableExistsAsync(tableId);
        await _unitOfWork.Tables.DeleteByIdAsync(tableId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateTableByIdAsync(int tableId, TableRequest updatedTable)
    {
        if (updatedTable is null)
        {
            throw new ArgumentNullException(nameof(updatedTable));
        }

        var existingTable = await EnsureTableExistsAsync(tableId);
        if (updatedTable.RestaurantId.HasValue)
        {
            await _restaurantService.EnsureRestaurantExistsAsync(updatedTable.RestaurantId.Value);
        }
        _mapper.Map(updatedTable, existingTable);
        await _unitOfWork.Tables.UpdateAsync(existingTable);
        await _unitOfWork.CommitAsync();
    }

    private static void ValidateTableRequest(TableRequest tableRequest)
    {
        ValidationHelper.EnsureNotNull(tableRequest, nameof(tableRequest));
        ValidationHelper.EnsureRequiredFields(
            (tableRequest.RestaurantId, nameof(tableRequest.RestaurantId))!,
            (tableRequest.TableNumber, nameof(tableRequest.TableNumber))!,
            (tableRequest.Capacity, nameof(tableRequest.Capacity))!
        );
    }
    
    private async Task<Table> EnsureTableExistsAsync(int tableId)
    {
        var existingTable = await _unitOfWork.Tables.GetByIdAsync(tableId);
        if (existingTable is null)
        {
            throw new NotFoundException($"The table doesn't exist");
        }

        return existingTable;
    }
}