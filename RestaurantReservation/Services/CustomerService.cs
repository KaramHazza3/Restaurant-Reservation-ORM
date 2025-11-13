using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Helpers;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }

    public async Task<List<CustomerResponse>> ListAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.ListAllAsync();
        return _mapper.Map<List<CustomerResponse>>(customers);
    }
    
    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest customerRequest)
    {
        ValidateCustomerRequest(customerRequest);
        
        var existingCustomer = await _unitOfWork.Customers.GetByEmailAsync(customerRequest.Email!);
        if (existingCustomer is not null)
        {
            throw new AlreadyExistsException($"The customer with email {customerRequest.Email} is already exists");
        }
        var customerEntity = _mapper.Map<Customer>(customerRequest);
        await _unitOfWork.Customers.CreateAsync(customerEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<CustomerResponse>(customerEntity);
    }

    public async Task<bool> DeleteCustomerByIdAsync(int customerId)
    {
        await EnsureCustomerExistsAsync(customerId);
        await _unitOfWork.Customers.DeleteByIdAsync(customerId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateCustomerByIdAsync(int customerId, CustomerRequest updatedCustomer)
    {
        if (updatedCustomer is null)
        {
            throw new ArgumentNullException(nameof(updatedCustomer));
        }

        var existingCustomer = await EnsureCustomerExistsAsync(customerId);
        _mapper.Map(updatedCustomer, existingCustomer);
        await _unitOfWork.Customers.UpdateAsync(existingCustomer);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Customer> EnsureCustomerExistsAsync(int customerId)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
        if (customer is null)
            throw new NotFoundException("The customer doesn't exist");

        return customer;
    }
    
    private static void ValidateCustomerRequest(CustomerRequest customerRequest)
    {
        ValidationHelper.EnsureNotNull(customerRequest, nameof(customerRequest));
        ValidationHelper.EnsureRequiredFields(
            (customerRequest.Email, nameof(customerRequest.Email))!,
            (customerRequest.FirstName, nameof(customerRequest.FirstName))!,
            (customerRequest.LastName, nameof(customerRequest.LastName))!,
            (customerRequest.PhoneNumber, nameof(customerRequest.PhoneNumber))!
            );
    }
    
}