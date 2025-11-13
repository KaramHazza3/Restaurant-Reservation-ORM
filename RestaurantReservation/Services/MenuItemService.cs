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

public class MenuItemService : IMenuItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public MenuItemService(IUnitOfWork unitOfWork, IRestaurantService restaurantService, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<MenuItemResponse>> ListAllMenuItemsAsync()
    {
        var menuItems = await _unitOfWork.MenuItems.ListAllAsync();
        return _mapper.Map<List<MenuItemResponse>>(menuItems);
    }
    
    public async Task<MenuItemResponse> CreateMenuItemAsync(MenuItemRequest menuItemRequest)
    {
        ValidateMenuItemRequest(menuItemRequest);
        await _restaurantService.EnsureRestaurantExistsAsync(menuItemRequest.RestaurantId!.Value);
        var menuItemEntity = _mapper.Map<MenuItem>(menuItemRequest);
        await _unitOfWork.MenuItems.CreateAsync(menuItemEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<MenuItemResponse>(menuItemEntity);
    }

    public async Task<bool> DeleteMenuItemByIdAsync(int menuItemId)
    {
        await EnsureMenuItemExistsAsync(menuItemId);
        await _unitOfWork.MenuItems.DeleteByIdAsync(menuItemId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateMenuItemByIdAsync(int menuItemId, MenuItemRequest updatedMenuItem)
    {
        if (updatedMenuItem is null)
        {
            throw new ArgumentNullException(nameof(updatedMenuItem));
        }

        var existingMenuItem = await EnsureMenuItemExistsAsync(menuItemId);
        if (updatedMenuItem.RestaurantId.HasValue)
        {
            await _restaurantService.EnsureRestaurantExistsAsync(updatedMenuItem.RestaurantId.Value);
        }

        _mapper.Map(updatedMenuItem, existingMenuItem);
        await _unitOfWork.MenuItems.UpdateAsync(existingMenuItem);
        await _unitOfWork.CommitAsync();
    }

    public async Task<MenuItem> EnsureMenuItemExistsAsync(int menuItemId)
    {
        var existingMenuItem = await _unitOfWork.MenuItems.GetByIdAsync(menuItemId);
        if (existingMenuItem is null)
        {
            throw new NotFoundException($"The menu item doesn't exist");
        }

        return existingMenuItem;
    }
    
    private static void ValidateMenuItemRequest(MenuItemRequest menuItemRequest)
    {
        ValidationHelper.EnsureNotNull(menuItemRequest, nameof(menuItemRequest));
        ValidationHelper.EnsureRequiredFields(
            (menuItemRequest.Description, nameof(menuItemRequest.Description))!,
            (menuItemRequest.Name, nameof(menuItemRequest.Name))!,
            (menuItemRequest.Price, nameof(menuItemRequest.Price))!,
            (menuItemRequest.RestaurantId, nameof(menuItemRequest.RestaurantId))!
            );
    }
    
}