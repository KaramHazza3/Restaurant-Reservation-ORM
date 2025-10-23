using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Contracts.Responses;
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

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(50);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);

            entity.HasIndex(c => c.Email).IsUnique();
        });
        
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(e => e.Restaurant)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RestaurantId);

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(30);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasOne(mi => mi.Restaurant)
                .WithMany(r => r.MenuItems)
                .HasForeignKey(mi => mi.RestaurantId);

            entity.Property(mi => mi.Description).IsRequired().HasMaxLength(300);
            entity.Property(mi => mi.Name).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
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

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            entity.HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
                

            entity.HasOne(r => r.Restaurant)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(r => r.Status).IsRequired().HasMaxLength(30);
            entity.Property(r => r.PartySize).IsRequired();
        });
        
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

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Address).IsRequired().HasMaxLength(100);
            entity.Property(r => r.PhoneNumber).IsRequired().HasMaxLength(100);
            entity.Property(r => r.OpeningHours).IsRequired().HasMaxLength(200);

        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId);
            
            entity.HasIndex(t => new { t.TableNumber, t.RestaurantId }).IsUnique();
        });
        
        var seedDate = new DateTime(2025, 10, 8, 12, 0, 0); // static timestamp

        // --- Customers ---
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", PhoneNumber = "111111", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Customer { Id = 2, FirstName = "Bob", LastName = "Brown", Email = "bob@example.com", PhoneNumber = "222222", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Customer { Id = 3, FirstName = "Charlie", LastName = "Davis", Email = "charlie@example.com", PhoneNumber = "333333", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Customer { Id = 4, FirstName = "Diana", LastName = "Evans", Email = "diana@example.com", PhoneNumber = "444444", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Customer { Id = 5, FirstName = "Ethan", LastName = "Foster", Email = "ethan@example.com", PhoneNumber = "555555", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- Restaurants ---
        modelBuilder.Entity<Restaurant>().HasData(
            new Restaurant { Id = 1, Name = "Korean BBQ", Address = "123 Main St", PhoneNumber = "100001", OpeningHours = "10:00-22:00", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Restaurant { Id = 2, Name = "Italian Bistro", Address = "456 Elm St", PhoneNumber = "100002", OpeningHours = "09:00-21:00", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Restaurant { Id = 3, Name = "Sushi House", Address = "789 Oak St", PhoneNumber = "100003", OpeningHours = "11:00-23:00", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Restaurant { Id = 4, Name = "Burger Place", Address = "321 Pine St", PhoneNumber = "100004", OpeningHours = "08:00-20:00", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Restaurant { Id = 5, Name = "Vegan Delight", Address = "654 Cedar St", PhoneNumber = "100005", OpeningHours = "10:00-20:00", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- Employees ---
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", Position = "Chef", RestaurantId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Doe", Position = "Manager", RestaurantId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Employee { Id = 3, FirstName = "Tom", LastName = "Lee", Position = "Waiter", RestaurantId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Employee { Id = 4, FirstName = "Sara", LastName = "Kim", Position = "Chef", RestaurantId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Employee { Id = 5, FirstName = "Mike", LastName = "Wong", Position = "Manager", RestaurantId = 4, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- Tables ---
        modelBuilder.Entity<Table>().HasData(
            new Table { Id = 1, TableNumber = 1, RestaurantId = 1, Capacity = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Table { Id = 2, TableNumber = 2, RestaurantId = 1, Capacity = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Table { Id = 3, TableNumber = 1, RestaurantId = 2, Capacity = 5, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Table { Id = 4, TableNumber = 1, RestaurantId = 3, Capacity = 4, CreatedAt = seedDate, UpdatedAt = seedDate },
            new Table { Id = 5, TableNumber = 2, RestaurantId = 3, Capacity = 7, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- MenuItems ---
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "Bibimbap", Description = "Korean mixed rice", RestaurantId = 1, Price = 12.5m, CreatedAt = seedDate, UpdatedAt = seedDate },
            new MenuItem { Id = 2, Name = "Bulgogi", Description = "Marinated beef", RestaurantId = 1, Price = 15m, CreatedAt = seedDate, UpdatedAt = seedDate },
            new MenuItem { Id = 3, Name = "Spaghetti", Description = "Classic pasta", RestaurantId = 2, Price = 10m, CreatedAt = seedDate, UpdatedAt = seedDate },
            new MenuItem { Id = 4, Name = "Sushi Roll", Description = "Fresh sushi", RestaurantId = 3, Price = 8.5m, CreatedAt = seedDate, UpdatedAt = seedDate },
            new MenuItem { Id = 5, Name = "Salad Bowl", Description = "Vegan salad", RestaurantId = 5, Price = 7m, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- Reservations ---
        modelBuilder.Entity<Reservation>().HasData(
            new Reservation { Id = 1, CustomerId = 1, RestaurantId = 1, PartySize = 4, Status = "Confirmed", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Reservation { Id = 2, CustomerId = 2, RestaurantId = 1, PartySize = 2, Status = "Pending", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Reservation { Id = 3, CustomerId = 3, RestaurantId = 2, PartySize = 8, Status = "Cancelled", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Reservation { Id = 4, CustomerId = 4, RestaurantId = 3, PartySize = 5, Status = "Confirmed", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Reservation { Id = 5, CustomerId = 5, RestaurantId = 4, PartySize = 3, Status = "Pending", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- Orders ---
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, ReservationId = 1, EmployeeId = 1, TotalPrice = 40m, Status = "Preparing", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Order { Id = 2, ReservationId = 2, EmployeeId = 2, TotalPrice = 30m, Status = "Served", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Order { Id = 3, ReservationId = 3, EmployeeId = 3, TotalPrice = 17m, Status = "Cancelled", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Order { Id = 4, ReservationId = 4, EmployeeId = 4, TotalPrice = 7m, Status = "Preparing", CreatedAt = seedDate, UpdatedAt = seedDate },
            new Order { Id = 5, ReservationId = 5, EmployeeId = 5, TotalPrice = 0m, Status = "Served", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- OrderItems ---
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, MenuItemId = 1, Quantity = 2, Notes = "No spice", CreatedAt = seedDate, UpdatedAt = seedDate },
            new OrderItem { Id = 2, OrderId = 1, MenuItemId = 2, Quantity = 1, Notes = "", CreatedAt = seedDate, UpdatedAt = seedDate },
            new OrderItem { Id = 3, OrderId = 2, MenuItemId = 3, Quantity = 3, Notes = "Extra cheese", CreatedAt = seedDate, UpdatedAt = seedDate },
            new OrderItem { Id = 4, OrderId = 3, MenuItemId = 4, Quantity = 2, Notes = "No wasabi", CreatedAt = seedDate, UpdatedAt = seedDate },
            new OrderItem { Id = 5, OrderId = 4, MenuItemId = 5, Quantity = 1, Notes = "Dressing on side", CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        // --- ReservationTables ---
        modelBuilder.Entity<ReservationTable>().HasData(
            new ReservationTable { ReservationId = 1, TableId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
            new ReservationTable { ReservationId = 1, TableId = 2, CreatedAt = seedDate, UpdatedAt = seedDate },
            new ReservationTable { ReservationId = 2, TableId = 1, CreatedAt = seedDate, UpdatedAt = seedDate },
            new ReservationTable { ReservationId = 3, TableId = 3, CreatedAt = seedDate, UpdatedAt = seedDate },
            new ReservationTable { ReservationId = 4, TableId = 4, CreatedAt = seedDate, UpdatedAt = seedDate }
        );

        modelBuilder.Entity<ReservationWithDetails>().HasNoKey().ToView("ReservationWithDetails");
        modelBuilder.Entity<EmployeeWithDetails>().HasNoKey().ToView("EmployeeWithDetails");
        modelBuilder.Entity<CustomersWithLargePartiesResponse>().HasNoKey();
        modelBuilder.HasDbFunction(() => CalculateRestaurantRevenue(default))
            .HasName("fn_CalculateRestaurantRevenue")
            .HasSchema("dbo");
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