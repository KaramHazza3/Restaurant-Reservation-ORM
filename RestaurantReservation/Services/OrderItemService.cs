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

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMenuItemService _menuItemService;
    private readonly IMapper _mapper;
    
    public OrderItemService(IUnitOfWork unitOfWork, IMenuItemService menuItemService, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._menuItemService = menuItemService;
        this._mapper = mapper;
    }
    
    public async Task<List<OrderItemResponse>> ListAllOrderItemsAsync()
    {
        var orderItems = await _unitOfWork.OrderItems.ListAllAsync();
        return _mapper.Map<List<OrderItemResponse>>(orderItems);
    }
    
    public async Task<OrderItemResponse> CreateOrderItemAsync(OrderItemRequest orderItemRequest)
    {
        ValidateOrderItemRequest(orderItemRequest);
        await _menuItemService.EnsureMenuItemExistsAsync(orderItemRequest.MenuItemId!.Value);
        await _menuItemService.EnsureMenuItemExistsAsync(orderItemRequest.MenuItemId!.Value);
        
        var orderItemEntity = _mapper.Map<OrderItem>(orderItemRequest);
        await _unitOfWork.OrderItems.CreateAsync(orderItemEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<OrderItemResponse>(orderItemEntity);
    }

    public async Task<bool> DeleteOrderItemByIdAsync(int orderItemId)
    {
        await EnsureOrderItemExists(orderItemId);
        await _unitOfWork.OrderItems.DeleteByIdAsync(orderItemId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateOrderItemByIdAsync(int orderItemId, OrderItemRequest updatedOrderItem)
    {
        if (updatedOrderItem is null)
        {
            throw new ArgumentNullException(nameof(updatedOrderItem));
        }

        var existingOrderItem = await EnsureOrderItemExists(orderItemId);
        if (updatedOrderItem.MenuItemId.HasValue)
        {
            await _menuItemService.EnsureMenuItemExistsAsync(updatedOrderItem.MenuItemId.Value);
        }
        _mapper.Map(updatedOrderItem, existingOrderItem);
        await _unitOfWork.OrderItems.UpdateAsync(existingOrderItem);
        await _unitOfWork.CommitAsync();
    }

    private async Task<OrderItem> EnsureOrderItemExists(int orderItemId)
    {
        var existingOrderItem = await _unitOfWork.OrderItems.GetByIdAsync(orderItemId);
        if (existingOrderItem is null)
        {
            throw new NotFoundException($"The order item doesn't exist");
        }

        return existingOrderItem;
    }
    private static void ValidateOrderItemRequest(OrderItemRequest orderItemRequest)
    {
        ValidationHelper.EnsureNotNull(orderItemRequest, nameof(orderItemRequest));
        ValidationHelper.EnsureRequiredFields(
            (orderItemRequest.Quantity, nameof(orderItemRequest.Quantity))!,
            (orderItemRequest.OrderId, nameof(orderItemRequest.OrderId))!,
            (orderItemRequest.MenuItemId, nameof(orderItemRequest.MenuItemId))!
            );
    }
    
}