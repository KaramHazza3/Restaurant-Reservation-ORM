namespace RestaurantReservation.Contracts.Responses;

public record MenuItemsListResponse(
    int ItemId,
    string ItemName,
    int Quantity,
    string Notes 
    );