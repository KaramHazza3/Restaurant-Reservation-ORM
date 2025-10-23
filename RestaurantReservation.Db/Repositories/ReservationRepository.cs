using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories;

public class ReservationRepository
{
    private readonly RestaurantReservationDbContext _context;

    public ReservationRepository(RestaurantReservationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
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

    public async Task<bool> DeleteByIdAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);
        if (reservation is null)
            return false;

        _context.Reservations.Remove(reservation);
        return true;
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Reservation>> GetReservationsByCustomerId(int customerId)
    {
        return await _context.Reservations.Where(r => r.CustomerId == customerId).ToListAsync();
    }
    
    public async Task<List<CustomersWithLargePartiesResponse>> ListCustomersReservationExceedsPartySize(int partySize)
    {
        return await _context.CustomersLargeReservations
            .FromSqlRaw("EXEC sp_ListCustomersExceedsPartySizeReservations @PartySize = {0}", partySize)
            .ToListAsync();
    }
}