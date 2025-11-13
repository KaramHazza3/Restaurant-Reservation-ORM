namespace RestaurantReservation.Contracts.Responses;

public record OrderItemResponse(
    int Id,
    int Quantity,
    string Notes,
    int MenuItemId,
    int OrderId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
