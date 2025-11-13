using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IRestaurantRepository
{
    Task<Restaurant> CreateAsync(Restaurant restaurant);
    Task<List<Restaurant>> ListAllAsync();
    Task<Restaurant?> GetByIdAsync(int restaurantId);
    Task DeleteByIdAsync(int restaurantId);
    Task UpdateAsync(Restaurant restaurant);
    Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
}