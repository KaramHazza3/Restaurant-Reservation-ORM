using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class ReservationTable : IHasTimestamps
{
    public int TableId { get; set; }
    public Table Table { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
    public int AssignedSeats { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}