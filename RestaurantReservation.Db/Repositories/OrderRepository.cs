using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository
{
    private readonly RestaurantReservationDbContext _context;

    public OrderRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Order> CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }
    
    public async Task<List<Order>> ListAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<bool> DeleteByIdAsync(int orderId)
    {
        var deletedCount = await _context.Orders
            .Where(o => o.Id == orderId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(Order order)
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Order>> GetOrdersWithItemsByReservationIdAsync(int reservationId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.MenuItem)
            .Where(o => o.ReservationId == reservationId)
            .ToListAsync();
    }
    
    public async Task<Decimal> CalculateAverageOrderAmountAsync(int employeeId)
    {
        var result = await _context.Orders
            .Where(o => o.EmployeeId == employeeId)
            .Select(x => x.TotalPrice)
            .ToListAsync();
        
        if (!result.Any())
            return 0m;

        return result.Sum() / result.Count;
    }
}