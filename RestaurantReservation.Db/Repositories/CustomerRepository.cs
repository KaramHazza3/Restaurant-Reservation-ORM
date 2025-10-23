using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository
{
    private readonly RestaurantReservationDbContext _context;

    public CustomerRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    
    public async Task<List<Customer>> ListAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string customerEmail)
    {
        return await _context.Customers.SingleOrDefaultAsync(c => c.Email == customerEmail);
    }
    
    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers.SingleOrDefaultAsync(c => c.Id == customerId);
    }

    public async Task<bool> DeleteByIdAsync(int customerId)
    {
        var deletedCount = await _context.Customers
            .Where(c => c.Id == customerId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _context.SaveChangesAsync();
    }
    
}