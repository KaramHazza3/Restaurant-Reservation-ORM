using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class OrderItemService
{
    private readonly OrderItemRepository _orderItemRepository;
    private readonly MenuItemService _menuItemService;
    private readonly IMapper _mapper;
    
    public OrderItemService(OrderItemRepository orderItemRepository, MenuItemService menuItemService, IMapper mapper)
    {
        this._orderItemRepository = orderItemRepository;
        this._menuItemService = menuItemService;
        this._mapper = mapper;
    }
    
    public async Task<List<OrderItemResponse>> ListAllOrderItemsAsync()
    {
        var orderItems = await _orderItemRepository.ListAllAsync();
        return _mapper.Map<List<OrderItemResponse>>(orderItems);
    }
    
    public async Task<OrderItemResponse> CreateOrderItemAsync(OrderItemRequest orderItemRequest)
    {
        if (orderItemRequest is null)
        {
            throw new ArgumentNullException(nameof(orderItemRequest));
        }

        if (orderItemRequest.MenuItemId is null ||
            orderItemRequest.OrderId is null ||
            orderItemRequest.Quantity is null)
        {
            throw new ArgumentException("All required order item fields (MenuItemId, OrderId, Quantity) must be provided.");
        }

        if (!await _menuItemService.IsMenuItemExists(orderItemRequest.MenuItemId.Value))
        {
            throw new NotFoundException("The menu item doesn't exist");
        }
        
        var orderItemEntity = _mapper.Map<OrderItem>(orderItemRequest);
        await _orderItemRepository.CreateAsync(orderItemEntity);
        return _mapper.Map<OrderItemResponse>(orderItemEntity);
    }

    public async Task<bool> DeleteOrderItemByIdAsync(int orderItemId)
    {
        var existingOrderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
        if (existingOrderItem is null)
        {
            throw new NotFoundException($"The order item doesn't exist");
        }
        return await _orderItemRepository.DeleteByIdAsync(orderItemId);
    }

    public async Task UpdateOrderItemByIdAsync(int orderItemId, OrderItemRequest updatedOrderItem)
    {
        if (updatedOrderItem is null)
        {
            throw new ArgumentNullException(nameof(updatedOrderItem));
        }
        var existingOrderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
        if (existingOrderItem is null)
        {
            throw new NotFoundException($"The order item doesn't exist");
        }

        if (updatedOrderItem.MenuItemId.HasValue &&
            !await _menuItemService.IsMenuItemExists(updatedOrderItem.MenuItemId.Value))
        {
            throw new NotFoundException("The menu item doesn't exist");
        }

        _mapper.Map(updatedOrderItem, existingOrderItem);
        await _orderItemRepository.UpdateAsync(existingOrderItem);
    }
}