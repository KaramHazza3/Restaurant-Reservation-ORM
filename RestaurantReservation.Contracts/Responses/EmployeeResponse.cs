namespace RestaurantReservation.Contracts.Responses;

public record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    string Position,
    int RestaurantId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);