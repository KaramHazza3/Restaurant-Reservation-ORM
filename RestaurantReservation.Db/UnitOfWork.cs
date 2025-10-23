using Microsoft.EntityFrameworkCore.Storage;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.Db;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantReservationDbContext _context;
    public ReservationTableRepository ReservationTable { get; }
    public ReservationRepository Reservations { get; }
    public TableRepository Tables { get; }
    
    public UnitOfWork(RestaurantReservationDbContext context)
    {
        _context = context;
        ReservationTable = new ReservationTableRepository(_context);
        Reservations = new ReservationRepository(_context);
        Tables = new TableRepository(_context);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}