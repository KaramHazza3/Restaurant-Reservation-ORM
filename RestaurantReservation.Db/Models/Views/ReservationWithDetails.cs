namespace RestaurantReservation.Db.Models.Views;

public class ReservationWithDetails
{
    public int ReservationId { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantAddress { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
}