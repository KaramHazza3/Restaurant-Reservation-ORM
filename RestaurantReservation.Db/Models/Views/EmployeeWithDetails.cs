namespace RestaurantReservation.Db.Models.Views;

public class EmployeeWithDetails
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string Position { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantAddress { get; set; }
}