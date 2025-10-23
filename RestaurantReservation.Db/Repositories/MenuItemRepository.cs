using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public MenuItemRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
        return menuItem;
    }
    
    public async Task<List<MenuItem>> ListAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int menuItemId)
    {
        return await _context.MenuItems.SingleOrDefaultAsync(mi => mi.Id == menuItemId);
    }

    public async Task<bool> DeleteByIdAsync(int menuItemId)
    {
        var deletedCount = await _context.MenuItems
            .Where(mi => mi.Id == menuItemId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        await _context.SaveChangesAsync();
    }
}