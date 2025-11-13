using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IOrderItemRepository
{
    Task<OrderItem> CreateAsync(OrderItem orderItem);
    Task<List<OrderItem>> ListAllAsync();
    Task<OrderItem?> GetByIdAsync(int orderItemId);
    Task DeleteByIdAsync(int orderItemId);
    Task UpdateAsync(OrderItem orderItem);
}