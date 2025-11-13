namespace RestaurantReservation.Contracts.Requests;

public record OrderItemRequest(
    int? MenuItemId,
    int? OrderId,
    int? Quantity,
    string? Notes
    );
