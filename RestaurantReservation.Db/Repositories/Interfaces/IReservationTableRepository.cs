using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.Intf;

public interface IReservationTableRepository
{
    Task CreateAsync(ReservationTable reservationTable);
    Task<List<ReservationTable>> GetByReservationIdAsync(int reservationId);
    Task DeleteAsync(ReservationTable reservationTable);
}