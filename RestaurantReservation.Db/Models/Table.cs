using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class Table : IHasTimestamps
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; } = true;
    public List<ReservationTable> ReservationTables { get; set; } = new List<ReservationTable>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}