using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class RestaurantConfiguration
{
    public static void ConfigureRestaurant(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Address).IsRequired().HasMaxLength(100);
            entity.Property(r => r.PhoneNumber).IsRequired().HasMaxLength(100);
            entity.Property(r => r.OpeningHours).IsRequired().HasMaxLength(200);
        });
    }
}