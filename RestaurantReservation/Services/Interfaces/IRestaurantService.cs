using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IRestaurantService
{
    Task<List<RestaurantResponse>> ListAllRestaurantsAsync();
    Task<RestaurantResponse> CreateRestaurantAsync(RestaurantRequest restaurantRequest);
    Task<bool> DeleteRestaurantByIdAsync(int restaurantId);
    Task UpdateRestaurantByIdAsync(int restaurantId, RestaurantRequest updatedRestaurant);
    Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId);
    Task<Restaurant> EnsureRestaurantExistsAsync(int restaurantId);
}