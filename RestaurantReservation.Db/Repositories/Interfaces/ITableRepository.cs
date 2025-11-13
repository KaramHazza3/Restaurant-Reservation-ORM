using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface ITableRepository
{
    Task<Table> CreateAsync(Table table);
    Task<List<Table>> ListAllAsync();
    Task<Table?> GetByIdAsync(int tableId);
    Task DeleteByIdAsync(int tableId);
    Task UpdateAsync(Table table);
    Task<List<Table>> GetAvailableTablesForRestaurant(int restaurantId);
}