using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Services.Interfaces;

public interface IMenuItemService
{
    Task<List<MenuItemResponse>> ListAllMenuItemsAsync();
    Task<MenuItemResponse> CreateMenuItemAsync(MenuItemRequest menuItemRequest);
    Task<bool> DeleteMenuItemByIdAsync(int menuItemId);
    Task UpdateMenuItemByIdAsync(int menuItemId, MenuItemRequest updatedMenuItem);
    Task<MenuItem> EnsureMenuItemExistsAsync(int menuItemId);
}