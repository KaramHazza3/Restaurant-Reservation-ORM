using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Contracts.Responses;

namespace RestaurantReservation.Db.Configurations;

public static class StoredProceduresConfiguration
{
    public static void ConfigureStoredProcedures(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomersWithLargePartiesResponse>().HasNoKey();
    }
}