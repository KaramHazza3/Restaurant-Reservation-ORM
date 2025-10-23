namespace RestaurantReservation.Contracts.Requests;

public record ReservationRequest(
    int? CustomerId,
    int? RestaurantId,
    int? PartySize,
    string Status = "Pending"
    );
