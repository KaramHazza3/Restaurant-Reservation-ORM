using RestaurantReservation.Shared.Enums;

namespace RestaurantReservation.Contracts.Requests;

public record OrderRequest(
    int? ReservationId,
    int? EmployeeId,
    decimal? TotalPrice,
    decimal? Discount,
    decimal? Tax,
    OrderStatus Status = OrderStatus.Preparing
    );