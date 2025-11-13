using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class TableConfiguration
{
    public static void ConfigureTable(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId);
            
            entity.HasIndex(t => new { t.TableNumber, t.RestaurantId }).IsUnique();
        });
    }
}