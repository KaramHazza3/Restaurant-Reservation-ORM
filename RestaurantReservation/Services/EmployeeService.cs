using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class EmployeeService
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly RestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public EmployeeService(EmployeeRepository employeeRepository, RestaurantService restaurantService, IMapper mapper)
    {
        this._employeeRepository = employeeRepository;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<EmployeeResponse>> ListAllEmployeesAsync()
    {
        var employees = await _employeeRepository.ListAllAsync();
        return _mapper.Map<List<EmployeeResponse>>(employees);
    }
    
    public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest employeeRequest)
    {
        if (employeeRequest is null)
        {
            throw new ArgumentNullException(nameof(employeeRequest));
        }

        if (string.IsNullOrEmpty(employeeRequest.Position) ||
            string.IsNullOrEmpty(employeeRequest.FirstName) ||
            string.IsNullOrEmpty(employeeRequest.LastName) ||
            employeeRequest.RestaurantId is null)
        {
            throw new ArgumentException("All required employee fields (FirstName, LastName, Position, RestaurantId) must be provided.");
        }

        if (!await _restaurantService.IsRestaurantExists(employeeRequest.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        var employeeEntity = _mapper.Map<Employee>(employeeRequest);
        await _employeeRepository.CreateAsync(employeeEntity);
        return _mapper.Map<EmployeeResponse>(employeeEntity);
    }

    public async Task<bool> DeleteEmployeeByIdAsync(int employeeId)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
        if (existingEmployee is null)
        {
            throw new NotFoundException($"The employee doesn't exist");
        }
        return await _employeeRepository.DeleteByIdAsync(employeeId);
    }

    public async Task UpdateEmployeeByIdAsync(int employeeId, EmployeeRequest updatedEmployee)
    {
        if (updatedEmployee is null)
        {
            throw new ArgumentNullException(nameof(updatedEmployee));
        }
        var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
        if (existingEmployee is null)
        {
            throw new NotFoundException($"The employee doesn't exist");
        }
        
        if(updatedEmployee.RestaurantId.HasValue && !await _restaurantService.IsRestaurantExists(updatedEmployee.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        _mapper.Map(updatedEmployee, existingEmployee);
        await _employeeRepository.UpdateAsync(existingEmployee);
    }

    public async Task<List<EmployeeResponse>> ListAllManagers()
    {
        var managers = await this._employeeRepository.ListManagersAsync();
        return _mapper.Map<List<EmployeeResponse>>(managers);
    }
    
    public async Task<bool> IsEmployeeExists(int employeeId)
    {
        return await _employeeRepository.GetByIdAsync(employeeId) != null;
    }
}