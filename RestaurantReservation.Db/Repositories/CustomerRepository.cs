using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly RestaurantReservationDbContext _context;

    public CustomerRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
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

    public async Task DeleteByIdAsync(int customerId)
    {
        await _context.Customers
            .Where(c => c.Id == customerId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customer.Id);
        
        _context.Entry(existingCustomer!).CurrentValues.SetValues(customer);
    }
    
}