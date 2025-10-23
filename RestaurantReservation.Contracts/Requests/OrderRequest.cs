namespace RestaurantReservation.Contracts.Requests;

public record OrderRequest(
    int? ReservationId,
    int? EmployeeId,
    decimal? TotalPrice,
    decimal? Discount,
    decimal? Tax,
    string Status = "Preparing"
    );