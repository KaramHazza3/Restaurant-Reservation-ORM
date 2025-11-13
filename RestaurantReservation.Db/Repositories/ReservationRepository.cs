using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.Intf;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RestaurantReservationDbContext _context;

    public ReservationRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        return reservation;
    }
    
    public async Task<List<Reservation>> ListAllAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int reservationId)
    {
        return await _context.Reservations.SingleOrDefaultAsync(r => r.Id == reservationId);
    }

    public async Task DeleteByIdAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation is null)
            return;

        _context.Reservations.Remove(reservation);
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        var existingReservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservation.Id);
        
        _context.Entry(existingReservation!).CurrentValues.SetValues(reservation);
    }
    
    public async Task<List<Reservation>> GetReservationsByCustomerIdAsync(int customerId)
    {
        return await _context.Reservations.Where(r => r.CustomerId == customerId).ToListAsync();
    }
    
    public async Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySizeAsync(int partySize)
    {
        return await _context.CustomersLargeReservations
            .FromSqlRaw("EXEC sp_ListCustomersExceedsPartySizeReservations @PartySize = {0}", partySize)
            .ToListAsync();
    }
}