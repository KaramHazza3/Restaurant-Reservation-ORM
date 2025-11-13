namespace RestaurantReservation.Contracts.Responses;

public record RestaurantResponse(
    int Id,
    string Name,
    string Address,
    string PhoneNumber,
    string OpeningHours,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
