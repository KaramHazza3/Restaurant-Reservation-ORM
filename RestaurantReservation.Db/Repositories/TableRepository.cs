using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class TableRepository : ITableRepository
{
    private readonly RestaurantReservationDbContext _context;

    public TableRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Table> CreateAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
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

    public async Task DeleteByIdAsync(int tableId)
    {
        await _context.Tables
            .Where(t => t.Id == tableId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Table table)
    {
        var existingTable = await _context.Tables
            .FirstOrDefaultAsync(t => t.Id == table.Id);
        
        _context.Entry(existingTable!).CurrentValues.SetValues(table);
    }

    public async Task<List<Table>> GetAvailableTablesForRestaurant(int restaurantId)
    {
        return await _context.Tables
            .Where(t => t.RestaurantId == restaurantId && t.IsAvailable == true)
            .ToListAsync();
    }
    
}