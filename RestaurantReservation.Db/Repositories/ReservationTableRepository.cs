using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class ReservationTableRepository : IReservationTableRepository
{
    private readonly RestaurantReservationDbContext _context;

    public ReservationTableRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }

    public async Task CreateAsync(ReservationTable reservationTable)
    {
        await _context.ReservationTables.AddAsync(reservationTable);
    }

    public async Task<List<ReservationTable>> GetByReservationIdAsync(int reservationId)
    {
        return await _context.ReservationTables.Where(rt => rt.ReservationId == reservationId).ToListAsync();
    }

    public Task DeleteAsync(ReservationTable reservationTable)
    {
        _context.ReservationTables.Remove(reservationTable);
        return Task.CompletedTask;
    }
}