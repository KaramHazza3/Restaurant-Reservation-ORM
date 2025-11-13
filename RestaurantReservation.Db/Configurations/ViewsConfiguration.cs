using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Configurations;

public static class ViewsConfiguration
{
    public static void ConfigureViews(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationWithDetails>().HasNoKey().ToView("ReservationWithDetails");
        modelBuilder.Entity<EmployeeWithDetails>().HasNoKey().ToView("EmployeeWithDetails");
    }
}