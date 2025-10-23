namespace RestaurantReservation.Contracts.Responses;

public record CustomersWithLargePartiesResponse(
    int Id,
    string Name,
    string Email,
    int PartySize
);