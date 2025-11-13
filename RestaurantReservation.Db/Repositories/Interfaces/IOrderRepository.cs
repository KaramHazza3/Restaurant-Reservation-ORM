using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<List<Order>> ListAllAsync();
    Task<Order?> GetByIdAsync(int orderId);
    Task DeleteByIdAsync(int orderId);
    Task UpdateAsync(Order order);
    Task<List<Order>> GetOrdersWithItemsByReservationIdAsync(int reservationId);
    Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
}