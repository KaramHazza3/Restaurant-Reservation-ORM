using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class Order : IHasTimestamps
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public Decimal TotalPrice { get; set; }
    public Decimal Discount { get; set; }
    public Decimal Tax { get; set; }
    public string Status { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}