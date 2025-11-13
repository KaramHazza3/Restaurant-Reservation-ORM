using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface ICustomerRepository
{
    Task<Customer> CreateAsync(Customer customer);
    Task<List<Customer>> ListAllAsync();
    Task<Customer?> GetByEmailAsync(string customerEmail);
    Task<Customer?> GetByIdAsync(int customerId);
    Task DeleteByIdAsync(int customerId);
    Task UpdateAsync(Customer customer);
}