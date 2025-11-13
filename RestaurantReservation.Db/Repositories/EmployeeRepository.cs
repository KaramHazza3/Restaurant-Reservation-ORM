using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;
using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Db.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly RestaurantReservationDbContext _context;

    public EmployeeRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Employee> CreateAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
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

    public async Task DeleteByIdAsync(int employeeId)
    {
        await _context.Employees
            .Where(e => e.Id == employeeId)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == employee.Id);
        
        _context.Entry(existingEmployee!).CurrentValues.SetValues(employee);
    }
    
    public async Task<List<Employee>> ListManagersAsync()
    {
        return await _context.Employees.Where(e => e.Position == EmployeePosition.Manager).ToListAsync();
    }
}