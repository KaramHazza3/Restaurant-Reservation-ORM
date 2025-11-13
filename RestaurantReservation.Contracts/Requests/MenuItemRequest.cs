namespace RestaurantReservation.Contracts.Requests;

public record MenuItemRequest(
    string? Name,
    string? Description,
    decimal? Price,
    int? RestaurantId
    );
