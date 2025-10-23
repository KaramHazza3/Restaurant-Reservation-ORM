namespace RestaurantReservation.Contracts.Requests;

public record RestaurantRequest(
    string? Name,
    string? Address,
    string? PhoneNumber,
    string? OpeningHours
    );