using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly EmployeeService _employeeService;
    private readonly ReservationService _reservationService;
    private readonly IMapper _mapper;
    
    public OrderService(OrderRepository orderRepository,
        EmployeeService employeeService, ReservationService reservationService,
        IMapper mapper)
    {
        this._orderRepository = orderRepository;
        this._employeeService = employeeService;
        this._reservationService = reservationService;
        this._mapper = mapper;
    }
    
    public async Task<List<OrderResponse>> ListAllOrderAsync()
    {
        var orders = await _orderRepository.ListAllAsync();
        return _mapper.Map<List<OrderResponse>>(orders);
    }
    
    public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
    {
        if (orderRequest is null)
        {
            throw new ArgumentNullException(nameof(orderRequest));
        }

        if (orderRequest.Discount is null ||
            orderRequest.Tax is null ||
            orderRequest.TotalPrice is null ||
            orderRequest.EmployeeId is null ||
            orderRequest.ReservationId is null)
        {
            throw new ArgumentException("All required order fields (ReservationId, EmployeeId, TotalPrice, Tax, Discount) must be provided.");
        }

        if (!await this._employeeService.IsEmployeeExists(orderRequest.EmployeeId.Value))
        {
            throw new NotFoundException("The employee doesn't exist");
        }
        
        if (!await this._reservationService.IsReservationExists(orderRequest.ReservationId.Value))
        {
            throw new NotFoundException("The reservation doesn't exist");
        }
        
        var orderEntity = _mapper.Map<Order>(orderRequest);
        await _orderRepository.CreateAsync(orderEntity);
        return _mapper.Map<OrderResponse>(orderEntity);
    }

    public async Task<bool> DeleteOrderByIdAsync(int orderId)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(orderId);
        if (existingOrder is null)
        {
            throw new NotFoundException($"The order doesn't exist");
        }
        return await _orderRepository.DeleteByIdAsync(orderId);
    }

    public async Task UpdateOrderByIdAsync(int orderId, OrderRequest updatedOrder)
    {
        if (updatedOrder is null)
        {
            throw new ArgumentNullException(nameof(updatedOrder));
        }
        var existingOrder = await _orderRepository.GetByIdAsync(orderId);
        if (existingOrder is null)
        {
            throw new NotFoundException($"The order doesn't exist");
        }

        if (updatedOrder.EmployeeId.HasValue &&
            !await this._employeeService.IsEmployeeExists(updatedOrder.EmployeeId.Value))
        {
            throw new NotFoundException("The employee doesn't exist");
        }
        
        if (updatedOrder.ReservationId.HasValue &&
            !await this._reservationService.IsReservationExists(updatedOrder.ReservationId.Value))
        {
            throw new NotFoundException("The reservation doesn't exist");
        }

        _mapper.Map(updatedOrder, existingOrder);
        await _orderRepository.UpdateAsync(existingOrder);
    }

    public async Task<List<OrdersAndMenuItemsResponse>> ListOrdersAndMenuItemsForReservationIdAsync(int reservationId)
    {
        var orders = await _orderRepository.GetOrdersWithItemsByReservationIdAsync(reservationId);

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
        var orders = await _orderRepository.GetOrdersWithItemsByReservationIdAsync(reservationId);

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
        return await this._orderRepository.CalculateAverageOrderAmountAsync(employeeId);
    }
}