namespace RestaurantReservation.Contracts.Requests;

public record CustomerRequest(
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber
    );