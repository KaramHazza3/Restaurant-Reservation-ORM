using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly RestaurantReservationDbContext _context;

    public OrderRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Order> CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
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

    public async Task DeleteByIdAsync(int orderId)
    {
        await _context.Orders
            .Where(o => o.Id == orderId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        var existingOrder = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == order.Id);
        
        _context.Entry(existingOrder!).CurrentValues.SetValues(order);
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