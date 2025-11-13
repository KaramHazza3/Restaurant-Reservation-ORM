using Microsoft.EntityFrameworkCore.Storage;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db;

public interface IUnitOfWork : IDisposable
{
    public IReservationTableRepository ReservationTable { get; }
    public IReservationRepository Reservations { get; }
    public ITableRepository Tables { get; }
    public ICustomerRepository Customers { get; }
    public IEmployeeRepository Employees { get; }
    public IMenuItemRepository MenuItems { get; }
    public IOrderRepository Orders { get; }
    public IOrderItemRepository OrderItems { get; }
    public IRestaurantRepository Restaurants { get; }
    Task<int> CommitAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}