namespace RestaurantReservation.Contracts.Responses;

public record ReservationResponse(
    int Id,
    int PartySize,
    int CustomerId,
    int RestaurantId,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
