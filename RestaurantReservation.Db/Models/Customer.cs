using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class Customer : IHasTimestamps
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}