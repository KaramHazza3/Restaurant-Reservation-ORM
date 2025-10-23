using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository
{
    private readonly RestaurantReservationDbContext _context;

    public TableRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Table> CreateAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
        await _context.SaveChangesAsync();
        return table;
    }
    
    public async Task<List<Table>> ListAllAsync()
    {
        return await _context.Tables.ToListAsync();
    }

    public async Task<Table?> GetByIdAsync(int tableId)
    {
        return await _context.Tables.SingleOrDefaultAsync(t => t.Id == tableId);
    }

    public async Task<bool> DeleteByIdAsync(int tableId)
    {
        var deletedCount = await _context.Tables
            .Where(t => t.Id == tableId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(Table table)
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Table>> GetAvailableTablesForRestaurant(int restaurantId)
    {
        return await _context.Tables
            .Where(t => t.RestaurantId == restaurantId && t.IsAvailable == true)
            .ToListAsync();
    }
    
}