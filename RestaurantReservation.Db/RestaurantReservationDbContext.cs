using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Configurations;
using RestaurantReservation.Db.Extentions;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Intf;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
    public RestaurantReservationDbContext() { } 

    public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options)
        : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationTable> ReservationTables { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<ReservationWithDetails> ReservationWithDetails { get; set; }
    public DbSet<EmployeeWithDetails> EmployeeWithDetails { get; set; }
    public DbSet<CustomersWithLargePartiesResponse> CustomersLargeReservations { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCustomer();
        modelBuilder.ConfigureEmployee();
        modelBuilder.ConfigureMenuItem();
        modelBuilder.ConfigureOrder();
        modelBuilder.ConfigureOrderItem();
        modelBuilder.ConfigureReservation();
        modelBuilder.ConfigureReservationTable();
        modelBuilder.ConfigureRestaurant();
        modelBuilder.ConfigureTable();
        modelBuilder.ConfigureViews();
        modelBuilder.ConfigureStoredProcedures();
        modelBuilder.HasDbFunction(() => CalculateRestaurantRevenue(default))
            .HasName("fn_CalculateRestaurantRevenue")
            .HasSchema("dbo");
        modelBuilder.Seed();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IHasTimestamps &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (IHasTimestamps)entry.Entity;
            if (entry.State == EntityState.Added)
                entity.CreatedAt = DateTime.UtcNow;

            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
    
    public decimal CalculateRestaurantRevenue(int restaurantId)
        => throw new NotSupportedException("Direct calls not supported — use in LINQ only.");
    

}