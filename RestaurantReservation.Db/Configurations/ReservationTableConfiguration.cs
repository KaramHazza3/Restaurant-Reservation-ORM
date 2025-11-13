using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class ReservationTableConfiguration
{
    public static void ConfigureReservationTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationTable>(entity =>
        {
            entity.HasKey(rt => new { rt.ReservationId, rt.TableId });
            
            entity.HasOne(rt => rt.Table)
                .WithMany(t => t.ReservationTables)
                .HasForeignKey(rt => rt.TableId)
                .OnDelete(DeleteBehavior.NoAction);
            
            entity.HasOne(rt => rt.Reservation)
                .WithMany(r => r.ReservationTables)
                .HasForeignKey(rt => rt.ReservationId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}