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

public class RestaurantService : IRestaurantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public RestaurantService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this._unitOfWork = unitOfWork;
        this._mapper = mapper;
    }
    
    public async Task<List<RestaurantResponse>> ListAllRestaurantsAsync()
    {
        var restaurants = await _unitOfWork.Restaurants.ListAllAsync();
        return _mapper.Map<List<RestaurantResponse>>(restaurants);
    }
    
    public async Task<RestaurantResponse> CreateRestaurantAsync(RestaurantRequest restaurantRequest)
    {
        ValidateRestaurantRequest(restaurantRequest);
        
        var restaurantEntity = _mapper.Map<Restaurant>(restaurantRequest);
        await _unitOfWork.Restaurants.CreateAsync(restaurantEntity);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<RestaurantResponse>(restaurantEntity);
    }

    public async Task<bool> DeleteRestaurantByIdAsync(int restaurantId)
    {
        await EnsureRestaurantExistsAsync(restaurantId);
        await _unitOfWork.Restaurants.DeleteByIdAsync(restaurantId);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task UpdateRestaurantByIdAsync(int restaurantId, RestaurantRequest updatedRestaurant)
    {
        if (updatedRestaurant is null)
        {
            throw new ArgumentNullException(nameof(updatedRestaurant));
        }

        var existingRestaurant = await EnsureRestaurantExistsAsync(restaurantId);

        _mapper.Map(updatedRestaurant, existingRestaurant);
        await _unitOfWork.Restaurants.UpdateAsync(existingRestaurant);
        await _unitOfWork.CommitAsync();
    }
    
    public async Task<Decimal> CalculateRestaurantRevenueAsync(int restaurantId)
    {
        return await this._unitOfWork.Restaurants.CalculateRestaurantRevenueAsync(restaurantId);
    }
    
    public async Task<Restaurant> EnsureRestaurantExistsAsync(int restaurantId)
    {
        var existingRestaurant = await _unitOfWork.Restaurants.GetByIdAsync(restaurantId);
        if (existingRestaurant is null)
        {
            throw new NotFoundException($"The restaurant doesn't exist");
        }

        return existingRestaurant;
    }
    
    private static void ValidateRestaurantRequest(RestaurantRequest restaurantRequest)
    {
        ValidationHelper.EnsureNotNull(restaurantRequest, nameof(restaurantRequest));
        ValidationHelper.EnsureRequiredFields(
            (restaurantRequest.Name, nameof(restaurantRequest.Name))!,
            (restaurantRequest.Address, nameof(restaurantRequest.Address))!,
            (restaurantRequest.OpeningHours, nameof(restaurantRequest.OpeningHours))!,
            (restaurantRequest.PhoneNumber, nameof(restaurantRequest.PhoneNumber))!
            );
    }
}