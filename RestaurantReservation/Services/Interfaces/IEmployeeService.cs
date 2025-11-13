using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeResponse>> ListAllEmployeesAsync();
    Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest employeeRequest);
    Task<bool> DeleteEmployeeByIdAsync(int employeeId);
    Task UpdateEmployeeByIdAsync(int employeeId, EmployeeRequest updatedEmployee);
    Task<List<EmployeeResponse>> ListAllManagers();
    Task<Employee> EnsureEmployeeExistsAsync(int employeeId);
}