using RestaurantReservation.Db.Models.Intf;
using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Db.Models;

public class Reservation : IHasTimestamps
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; }
    public List<ReservationTable> ReservationTables { get; set; } = new List<ReservationTable>();
    public List<Order> Orders { get; set; } = new List<Order>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}