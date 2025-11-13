namespace RestaurantReservation.Contracts.Responses;

public record OrderResponse(
    int Id,
    int ReservationId,
    int EmployeeId,
    decimal TotalPrice,
    decimal Discount,
    decimal Tax,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
