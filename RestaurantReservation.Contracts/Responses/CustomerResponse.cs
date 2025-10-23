namespace RestaurantReservation.Contracts.Responses;

public record CustomerResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );