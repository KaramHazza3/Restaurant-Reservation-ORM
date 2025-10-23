using RestaurantReservation.Db.Models.Intf;

namespace RestaurantReservation.Db.Models;

public class OrderItem : IHasTimestamps
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}