using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IOrderItemService
{
    Task<List<OrderItemResponse>> ListAllOrderItemsAsync();
    Task<OrderItemResponse> CreateOrderItemAsync(OrderItemRequest orderItemRequest);
    Task<bool> DeleteOrderItemByIdAsync(int orderItemId);
    Task UpdateOrderItemByIdAsync(int orderItemId, OrderItemRequest updatedOrderItem);
}