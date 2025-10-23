namespace RestaurantReservation.Contracts.Responses;

public record TableResponse(
    int Id,
    int TableNumber,
    int RestaurantId,
    int Capacity,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
