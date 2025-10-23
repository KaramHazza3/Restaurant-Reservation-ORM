namespace RestaurantReservation.Contracts.Responses;

public record MenuItemResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int RestaurantId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
