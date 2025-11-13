namespace RestaurantReservation.Contracts.Responses;

public record OrdersAndMenuItemsResponse(
    int ReservationId,
    int OrderId,
    List<MenuItemsListResponse> MenuItems
    );