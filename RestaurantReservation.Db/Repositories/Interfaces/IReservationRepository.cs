using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IReservationRepository
{
    Task<Reservation> CreateAsync(Reservation reservation);
    Task<List<Reservation>> ListAllAsync();
    Task<Reservation?> GetByIdAsync(int reservationId);
    Task DeleteByIdAsync(int reservationId);
    Task UpdateAsync(Reservation reservation);
    Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId);
    Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySizeAsync(int partySize);
}