using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class CustomerConfiguration
{
    public static void ConfigureCustomer(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(50);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);

            entity.HasIndex(c => c.Email).IsUnique();
        });
    }
}