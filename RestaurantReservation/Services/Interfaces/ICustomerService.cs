using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerResponse>> ListAllCustomersAsync();
    Task<CustomerResponse> CreateCustomerAsync(CustomerRequest customerRequest);
    Task<bool> DeleteCustomerByIdAsync(int customerId);
    Task UpdateCustomerByIdAsync(int customerId, CustomerRequest updatedCustomer);
    Task<Customer> EnsureCustomerExistsAsync(int customerId);
}