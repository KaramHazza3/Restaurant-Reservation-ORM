using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IMenuItemRepository
{
    Task<MenuItem> CreateAsync(MenuItem menuItem);
    Task<List<MenuItem>> ListAllAsync();
    Task<MenuItem?> GetByIdAsync(int menuItemId);
    Task DeleteByIdAsync(int menuItemId);
    Task UpdateAsync(MenuItem menuItem);
}