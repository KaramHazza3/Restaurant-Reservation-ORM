using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class ReservationConfiguration
{
    public static void ConfigureReservation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(r => r.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);
            
            entity.HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
                

            entity.HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(r => r.Status).IsRequired().HasMaxLength(30);
            entity.Property(r => r.PartySize).IsRequired();
        });
    }
}