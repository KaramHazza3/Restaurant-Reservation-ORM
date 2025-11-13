using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class Restaurant : IHasTimestamps
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public List<Table> Tables { get; set; } = new List<Table>();
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}