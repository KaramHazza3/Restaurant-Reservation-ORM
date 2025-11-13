using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly RestaurantReservationDbContext _context;

    public RestaurantRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        return restaurant;
    }
    
    public async Task<List<Restaurant>> ListAllAsync()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int restaurantId)
    {
        return await _context.Restaurants.SingleOrDefaultAsync(r => r.Id == restaurantId);
    }

    public async Task DeleteByIdAsync(int restaurantId)
    {
        await _context.Restaurants
            .Where(r => r.Id == restaurantId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        var existingRestaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurant.Id);
        
        _context.Entry(existingRestaurant!).CurrentValues.SetValues(restaurant);
    }
    
    public async Task<decimal> CalculateRestaurantRevenueAsync(int restaurantId)
    {
        return await _context.Restaurants
            .Where(r => r.Id == restaurantId)
            .Select(r => _context.CalculateRestaurantRevenue(r.Id))
            .FirstOrDefaultAsync();
    }
}