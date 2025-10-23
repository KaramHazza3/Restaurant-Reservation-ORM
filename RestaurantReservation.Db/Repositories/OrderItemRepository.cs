using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public OrderItemRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }
    
    public async Task<List<OrderItem>> ListAllAsync()
    {
        return await _context.OrderItems.ToListAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int orderItemId)
    {
        return await _context.OrderItems.SingleOrDefaultAsync(oi => oi.Id == orderItemId);
    }

    public async Task<bool> DeleteByIdAsync(int orderItemId)
    {
        var deletedCount = await _context.OrderItems
            .Where(oi => oi.Id == orderItemId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        await _context.SaveChangesAsync();
    }
}