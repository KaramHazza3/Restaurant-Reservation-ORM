using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository
{
    private readonly RestaurantReservationDbContext _context;

    public EmployeeRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
    
    public async Task<List<Employee>> ListAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId)
    {
        return await _context.Employees.SingleOrDefaultAsync(e => e.Id == employeeId);
    }

    public async Task<bool> DeleteByIdAsync(int employeeId)
    {
        var deletedCount = await _context.Employees
            .Where(e => e.Id == employeeId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    public async Task UpdateAsync(Employee employee)
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Employee>> ListManagersAsync()
    {
        return await _context.Employees.Where(e => e.Position == "Manager").ToListAsync();
    }
}