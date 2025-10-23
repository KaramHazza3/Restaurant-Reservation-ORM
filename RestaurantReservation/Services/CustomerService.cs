using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(CustomerRepository customerRepository, IMapper mapper)
    {
        this._customerRepository = customerRepository;
        this._mapper = mapper;
    }

    public async Task<List<CustomerResponse>> ListAllCustomersAsync()
    {
        var customers = await _customerRepository.ListAllAsync();
        return _mapper.Map<List<CustomerResponse>>(customers);
    }
    
    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest customerRequest)
    {
        if (customerRequest is null)
        {
            throw new ArgumentNullException(nameof(customerRequest));
        }

        if (string.IsNullOrEmpty(customerRequest.Email) ||
            string.IsNullOrEmpty(customerRequest.FirstName) ||
            string.IsNullOrEmpty(customerRequest.LastName) ||
            string.IsNullOrEmpty(customerRequest.PhoneNumber))
        {
            throw new ArgumentException("All required customer fields (Email, FirstName, LastName, PhoneNumber) must be provided.");
        }
        
        var existingCustomer = await _customerRepository.GetByEmailAsync(customerRequest.Email);
        if (existingCustomer is not null)
        {
            throw new AlreadyExistsException($"The customer with email {customerRequest.Email} is already exists");
        }
        var customerEntity = _mapper.Map<Customer>(customerRequest);
        await _customerRepository.CreateAsync(customerEntity);
        return _mapper.Map<CustomerResponse>(customerEntity);
    }

    public async Task<bool> DeleteCustomerByIdAsync(int customerId)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
        if (existingCustomer is null)
        {
            throw new NotFoundException($"The customer doesn't exist");
        }
        return await _customerRepository.DeleteByIdAsync(customerId);
    }

    public async Task UpdateCustomerByIdAsync(int customerId, CustomerRequest updatedCustomer)
    {
        if (updatedCustomer is null)
        {
            throw new ArgumentNullException(nameof(updatedCustomer));
        }
        var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
        if (existingCustomer is null)
        {
            throw new NotFoundException($"The customer doesn't exist");
        }

        _mapper.Map(updatedCustomer, existingCustomer);
        await _customerRepository.UpdateAsync(existingCustomer);
    }

    public async Task<bool> IsCustomerExists(int customerId)
    {
        return await _customerRepository.GetByIdAsync(customerId) != null;
    }
    
}