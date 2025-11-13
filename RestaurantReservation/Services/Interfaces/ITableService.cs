using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface ITableService
{
    Task<List<TableResponse>> ListAllTablesAsync();
    Task<TableResponse> CreateTableAsync(TableRequest tableRequest);
    Task<bool> DeleteTableByIdAsync(int tableId);
    Task UpdateTableByIdAsync(int tableId, TableRequest updatedTable);
}