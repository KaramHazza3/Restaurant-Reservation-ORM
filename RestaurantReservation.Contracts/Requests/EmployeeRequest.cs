
using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Contracts.Requests;

public record EmployeeRequest(
    string? FirstName,
    string? LastName,
    EmployeePosition? Position,
    int? RestaurantId
    );