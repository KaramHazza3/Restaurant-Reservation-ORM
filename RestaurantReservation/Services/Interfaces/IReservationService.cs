using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IReservationService
{
    Task<List<ReservationResponse>> ListAllReservationAsync();
    Task<ReservationResponse> CreateReservationAsync(ReservationRequest reservationRequest);
    Task<bool> DeleteReservationByIdAsync(int reservationId);
    Task UpdateReservationByIdAsync(int reservationId, ReservationRequest updatedReservation);
    Task<List<ReservationResponse>> GetReservationsByCustomerId(int customerId);
    Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySize(int partySize);
    Task<Reservation> EnsureReservationExistsAsync(int reservationId);
}