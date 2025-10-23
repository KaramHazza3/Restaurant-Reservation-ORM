using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class MenuItem : IHasTimestamps
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}