using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class Employee : IHasTimestamps
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Position { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}
