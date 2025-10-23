using Microsoft.EntityFrameworkCore.Storage;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.Db;

public interface IUnitOfWork : IDisposable
{
    ReservationTableRepository ReservationTable { get; }
    ReservationRepository Reservations { get; }
    TableRepository Tables { get; }
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}