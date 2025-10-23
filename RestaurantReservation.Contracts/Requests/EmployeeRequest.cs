namespace RestaurantReservation.Contracts.Requests;

public record EmployeeRequest(
    string? FirstName,
    string? LastName,
    string? Position,
    int? RestaurantId
    );