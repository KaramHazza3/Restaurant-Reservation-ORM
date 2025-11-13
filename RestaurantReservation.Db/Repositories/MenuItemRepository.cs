using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public MenuItemRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
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

    public async Task DeleteByIdAsync(int menuItemId)
    {
        await _context.MenuItems
            .Where(mi => mi.Id == menuItemId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        var existingMenuItem = await _context.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == menuItem.Id);
        
        _context.Entry(existingMenuItem!).CurrentValues.SetValues(menuItem);
    }
}