using Microsoft.EntityFrameworkCore.Storage;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantReservationDbContext _context;
    public IReservationTableRepository ReservationTable { get; }
    public IReservationRepository Reservations { get; }
    public ITableRepository Tables { get; }
    public ICustomerRepository Customers { get; }
    public IEmployeeRepository Employees { get; }
    public IMenuItemRepository MenuItems { get; }
    public IOrderRepository Orders { get; }
    public IOrderItemRepository OrderItems { get; }
    public IRestaurantRepository Restaurants { get; }

    public UnitOfWork(
        RestaurantReservationDbContext context,
        IReservationTableRepository reservationTable,
        IReservationRepository reservations,
        ITableRepository tables,
        ICustomerRepository customers,
        IEmployeeRepository employees,
        IMenuItemRepository menuItems,
        IOrderRepository orders,
        IOrderItemRepository orderItems,
        IRestaurantRepository restaurants)
    {
        _context = context;
        ReservationTable = reservationTable;
        Reservations = reservations;
        Tables = tables;
        Customers = customers;
        Employees = employees;
        MenuItems = menuItems;
        Orders = orders;
        OrderItems = orderItems;
        Restaurants = restaurants;
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}