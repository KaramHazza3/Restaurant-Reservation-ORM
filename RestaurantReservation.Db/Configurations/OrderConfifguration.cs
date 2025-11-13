using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class OrderConfifguration
{
    public static void ConfigureOrder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);
            
            entity.HasOne(o => o.Reservation)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.ReservationId)
                .OnDelete(DeleteBehavior.NoAction); 

            entity.HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction); 

            entity.Property(o => o.Status).IsRequired().HasMaxLength(50);
        });
    }
}