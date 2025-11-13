using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IEmployeeRepository
{
    Task<Employee> CreateAsync(Employee employee);
    Task<List<Employee>> ListAllAsync();
    Task<Employee?> GetByIdAsync(int employeeId);
    Task DeleteByIdAsync(int employeeId);
    Task UpdateAsync(Employee employee);
    Task<List<Employee>> ListManagersAsync();
}