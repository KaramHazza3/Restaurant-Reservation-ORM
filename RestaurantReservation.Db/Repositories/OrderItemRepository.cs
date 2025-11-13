using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public OrderItemRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<OrderItem> CreateAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
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

    public async Task DeleteByIdAsync(int orderItemId)
    {
        await _context.OrderItems
            .Where(oi => oi.Id == orderItemId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        var existingOrderItem = await _context.OrderItems
            .FirstOrDefaultAsync(oi => oi.Id == orderItem.Id);
        
        _context.Entry(existingOrderItem!).CurrentValues.SetValues(orderItem);
    }
}