using System.ComponentModel.DataAnnotations;
using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Intf;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Helpers;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public EmployeeService(IUnitOfWork unitOfWork, IRestaurantService restaurantService, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<EmployeeResponse>> ListAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.ListAllAsync();
        return _mapper.Map<List<EmployeeResponse>>(employees);
    }
    
    public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeRequest employeeRequest)
    {
        ValidateEmployeeRequest(employeeRequest);
        await _restaurantService.EnsureRestaurantExistsAsync(employeeRequest.RestaurantId!.Value);
        
        var employeeEntity = _mapper.Map<Employee>(employeeRequest);
        await _unitOfWork.Employees.CreateAsync(employeeEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<EmployeeResponse>(employeeEntity);
    }

    public async Task<bool> DeleteEmployeeByIdAsync(int employeeId)
    {
        await EnsureEmployeeExistsAsync(employeeId);
        await _unitOfWork.Employees.DeleteByIdAsync(employeeId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateEmployeeByIdAsync(int employeeId, EmployeeRequest updatedEmployee)
    {
        if (updatedEmployee is null)
        {
            throw new ArgumentNullException(nameof(updatedEmployee));
        }

        var existingEmployee = await EnsureEmployeeExistsAsync(employeeId);

        if (updatedEmployee.RestaurantId.HasValue)
        {
            await _restaurantService.EnsureRestaurantExistsAsync(updatedEmployee.RestaurantId.Value);
        }
        
        _mapper.Map(updatedEmployee, existingEmployee);
        await _unitOfWork.Employees.UpdateAsync(existingEmployee);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<EmployeeResponse>> ListAllManagers()
    {
        var managers = await this._unitOfWork.Employees.ListManagersAsync();
        return _mapper.Map<List<EmployeeResponse>>(managers);
    }
    
    public async Task<Employee> EnsureEmployeeExistsAsync(int employeeId)
    {
        var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
        if (existingEmployee is null)
        {
            throw new NotFoundException($"The employee doesn't exist");
        }

        return existingEmployee;
    }
    
    private static void ValidateEmployeeRequest(EmployeeRequest employeeRequest)
    {
        ValidationHelper.EnsureNotNull(employeeRequest, nameof(employeeRequest));
        ValidationHelper.EnsureRequiredFields(
            (employeeRequest.Position, nameof(employeeRequest.Position))!,
            (employeeRequest.FirstName, nameof(employeeRequest.FirstName))!,
            (employeeRequest.LastName, nameof(employeeRequest.LastName))!,
            (employeeRequest.RestaurantId, nameof(employeeRequest.RestaurantId))!
            );
    }
}