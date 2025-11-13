using System.ComponentModel.DataAnnotations;
using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Db.Repositories.Intf;
using RestaurantReservation.Exceptions;
using RestaurantReservation.Helpers;
using RestaurantReservation.Services.Interfaces;

namespace RestaurantReservation.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeService _employeeService;
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;
    
    public OrderService(IUnitOfWork unitOfWork,
        IEmployeeService employeeService, IReservationService reservationService,
        IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._employeeService = employeeService;
        this._reservationService = reservationService;
        this._mapper = mapper;
    }
    
    public async Task<List<OrderResponse>> ListAllOrderAsync()
    {
        var orders = await _unitOfWork.Orders.ListAllAsync();
        return _mapper.Map<List<OrderResponse>>(orders);
    }
    
    public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
    {
        ValidateOrderRequest(orderRequest);
        await _employeeService.EnsureEmployeeExistsAsync(orderRequest.EmployeeId!.Value);
        await _reservationService.EnsureReservationExistsAsync(orderRequest.ReservationId!.Value);
        
        var orderEntity = _mapper.Map<Order>(orderRequest);
        await _unitOfWork.Orders.CreateAsync(orderEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<OrderResponse>(orderEntity);
    }

    public async Task<bool> DeleteOrderByIdAsync(int orderId)
    {
        await EnsureOrderExistsAsync(orderId);
        await _unitOfWork.Orders.DeleteByIdAsync(orderId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateOrderByIdAsync(int orderId, OrderRequest updatedOrder)
    {
        if (updatedOrder is null)
        {
            throw new ArgumentNullException(nameof(updatedOrder));
        }

        var existingOrder = await EnsureOrderExistsAsync(orderId);
        if (updatedOrder.EmployeeId.HasValue)
        {
            await _employeeService.EnsureEmployeeExistsAsync(updatedOrder.EmployeeId.Value);
        }

        if (updatedOrder.ReservationId.HasValue)
        {
            await _reservationService.EnsureReservationExistsAsync(updatedOrder.ReservationId.Value);
        }

        _mapper.Map(updatedOrder, existingOrder);
        await _unitOfWork.Orders.UpdateAsync(existingOrder);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<OrdersAndMenuItemsResponse>> ListOrdersAndMenuItemsForReservationIdAsync(int reservationId)
    {
        var orders = await _unitOfWork.Orders.GetOrdersWithItemsByReservationIdAsync(reservationId);

        return orders.Select(o => new OrdersAndMenuItemsResponse(
            o.ReservationId,
            o.Id,
            o.OrderItems.Select(oi => new MenuItemsListResponse(
                oi.MenuItemId,
                oi.MenuItem.Name,
                oi.Quantity,
                oi.Notes
            )).ToList()
        )).ToList();
    }

    public async Task<List<MenuItemsListResponse>> ListOrderedMenuItemsForReservationIdAsync(int reservationId)
    {
        var orders = await _unitOfWork.Orders.GetOrdersWithItemsByReservationIdAsync(reservationId);

        return orders
            .SelectMany(o => o.OrderItems)
            .Select(oi => new MenuItemsListResponse(
                oi.MenuItemId,
                oi.MenuItem.Name,
                oi.Quantity,
                oi.Notes
            )).ToList();
    }

    public async Task<Decimal> CalculateAverageOrderAmountForEmployeeIdAsync(int employeeId)
    {
        return await this._unitOfWork.Orders.CalculateAverageOrderAmountAsync(employeeId);
    }
    
    private static void ValidateOrderRequest(OrderRequest orderRequest)
    {
       ValidationHelper.EnsureNotNull(orderRequest, nameof(orderRequest));
       ValidationHelper.EnsureRequiredFields(
           (orderRequest.Discount, nameof(orderRequest.Discount))!,
           (orderRequest.TotalPrice, nameof(orderRequest.TotalPrice))!,
           (orderRequest.Tax, nameof(orderRequest.Tax))!,
           (orderRequest.EmployeeId, nameof(orderRequest.EmployeeId))!,
           (orderRequest.ReservationId, nameof(orderRequest.ReservationId))!
           );
    }
    
    private async Task<Order> EnsureOrderExistsAsync(int orderId)
    {
        var existingOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (existingOrder is null)
        {
            throw new NotFoundException($"The order doesn't exist");
        }

        return existingOrder;
    }
}