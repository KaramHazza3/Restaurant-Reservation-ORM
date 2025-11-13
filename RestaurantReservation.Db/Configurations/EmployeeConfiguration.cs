using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Configurations;

public static class EmployeeConfiguration
{
    public static void ConfigureEmployee(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Position)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);
            
            entity.HasOne(e => e.Restaurant)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RestaurantId);

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(30);
        });
    }
}