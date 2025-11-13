using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderResponse>> ListAllOrderAsync();
    Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
    Task<bool> DeleteOrderByIdAsync(int orderId);
    Task UpdateOrderByIdAsync(int orderId, OrderRequest updatedOrder);
    Task<List<OrdersAndMenuItemsResponse>> ListOrdersAndMenuItemsForReservationIdAsync(int reservationId);
    Task<List<MenuItemsListResponse>> ListOrderedMenuItemsForReservationIdAsync(int reservationId);
    Task<decimal> CalculateAverageOrderAmountForEmployeeIdAsync(int employeeId);
}