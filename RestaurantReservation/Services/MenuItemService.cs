using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class MenuItemService
{
    private readonly MenuItemRepository _menuItemRepository;
    private readonly RestaurantService _restaurantService;
    private readonly IMapper _mapper;
    
    public MenuItemService(MenuItemRepository menuItemRepository, RestaurantService restaurantService, IMapper mapper)
    {
        this._menuItemRepository = menuItemRepository;
        this._restaurantService = restaurantService;
        this._mapper = mapper;
    }
    
    public async Task<List<MenuItemResponse>> ListAllMenuItemsAsync()
    {
        var menuItems = await _menuItemRepository.ListAllAsync();
        return _mapper.Map<List<MenuItemResponse>>(menuItems);
    }
    
    public async Task<MenuItemResponse> CreateMenuItemAsync(MenuItemRequest menuItemRequest)
    {
        if (menuItemRequest is null)
        {
            throw new ArgumentNullException(nameof(menuItemRequest));
        }

        if (string.IsNullOrEmpty(menuItemRequest.Name) ||
            string.IsNullOrEmpty(menuItemRequest.Description) ||
            menuItemRequest.Price is null ||
            menuItemRequest.RestaurantId is null)
        {
            throw new ArgumentException("All required menu item fields (Name, Description, Price, RestaurantId) must be provided.");
        }
        
        if (!await _restaurantService.IsRestaurantExists(menuItemRequest.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        var menuItemEntity = _mapper.Map<MenuItem>(menuItemRequest);
        await _menuItemRepository.CreateAsync(menuItemEntity);
        return _mapper.Map<MenuItemResponse>(menuItemEntity);
    }

    public async Task<bool> DeleteMenuItemByIdAsync(int menuItemId)
    {
        var existingMenuItem = await _menuItemRepository.GetByIdAsync(menuItemId);
        if (existingMenuItem is null)
        {
            throw new NotFoundException($"The menu item doesn't exist");
        }
        return await _menuItemRepository.DeleteByIdAsync(menuItemId);
    }

    public async Task UpdateMenuItemByIdAsync(int menuItemId, MenuItemRequest updatedMenuItem)
    {
        if (updatedMenuItem is null)
        {
            throw new ArgumentNullException(nameof(updatedMenuItem));
        }
        var existingMenuItem = await _menuItemRepository.GetByIdAsync(menuItemId);
        if (existingMenuItem is null)
        {
            throw new NotFoundException($"The menu item doesn't exist");
        }
        
        if(updatedMenuItem.RestaurantId.HasValue && !await _restaurantService.IsRestaurantExists(updatedMenuItem.RestaurantId.Value))
        {
            throw new NotFoundException("The restaurant doesn't exist");
        }
        
        _mapper.Map(updatedMenuItem, existingMenuItem);
        await _menuItemRepository.UpdateAsync(existingMenuItem);
    }
    
    public async Task<bool> IsMenuItemExists(int menuItemId)
    {
        return await _menuItemRepository.GetByIdAsync(menuItemId) != null;
    }
}