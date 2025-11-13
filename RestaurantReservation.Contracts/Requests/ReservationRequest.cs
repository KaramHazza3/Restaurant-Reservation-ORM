using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Contracts.Requests;

public record ReservationRequest(
    int? CustomerId,
    int? RestaurantId,
    int? PartySize,
    ReservationStatus Status = ReservationStatus.Pending
    );
