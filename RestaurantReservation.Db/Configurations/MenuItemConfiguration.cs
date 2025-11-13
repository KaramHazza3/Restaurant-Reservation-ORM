using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class MenuItemConfiguration
{
    public static void ConfigureMenuItem(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasOne(mi => mi.Restaurant)
                .WithMany(r => r.MenuItems)
                .HasForeignKey(mi => mi.RestaurantId);

            entity.Property(mi => mi.Description).IsRequired().HasMaxLength(300);
            entity.Property(mi => mi.Name).IsRequired().HasMaxLength(50);
        });
    }
}