namespace RestaurantReservation.Contracts.Requests;

public record TableRequest(
    int? TableNumber,
    int? RestaurantId,
    int? Capacity
    );
