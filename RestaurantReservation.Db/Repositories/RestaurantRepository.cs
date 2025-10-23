using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class RestaurantRepository
{
    private readonly RestaurantReservationDbContext _context;

    public RestaurantRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
        await _context.SaveChangesAsync();
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

    public async Task<bool> DeleteByIdAsync(int restaurantId)
    {
        var deletedCount = await _context.Restaurants
            .Where(r => r.Id == restaurantId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<Decimal> CalculateRestaurantRevenueAsync(int restaurantId)
    {
        return await _context.Restaurants
            .Where(r => r.Id == restaurantId)
            .Select(r => _context.CalculateRestaurantRevenue(restaurantId))
            .FirstOrDefaultAsync();
    }
}