using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.Exceptions;

namespace RestaurantReservation.Services;

public class RestaurantService
{
    private readonly RestaurantRepository _restaurantRepository;
    private readonly IMapper _mapper;
    
    public RestaurantService(RestaurantRepository restaurantRepository, IMapper mapper)
    {
        this._restaurantRepository = restaurantRepository;
        this._mapper = mapper;
    }
    
    public async Task<List<RestaurantResponse>> ListAllRestaurantsAsync()
    {
        var restaurants = await _restaurantRepository.ListAllAsync();
        return _mapper.Map<List<RestaurantResponse>>(restaurants);
    }
    
    public async Task<RestaurantResponse> CreateRestaurantAsync(RestaurantRequest restaurantRequest)
    {
        if (restaurantRequest is null)
        {
            throw new ArgumentNullException(nameof(restaurantRequest));
        }

        if (string.IsNullOrWhiteSpace(restaurantRequest.Name) ||
            string.IsNullOrWhiteSpace(restaurantRequest.Address) ||
            string.IsNullOrWhiteSpace(restaurantRequest.OpeningHours) ||
            string.IsNullOrWhiteSpace(restaurantRequest.PhoneNumber))
        {
            throw new ArgumentException("All required restaurant fields (Name, Address, OpeningHours, PhoneNumber) must be provided.");
        }
        
        var restaurantEntity = _mapper.Map<Restaurant>(restaurantRequest);
        await _restaurantRepository.CreateAsync(restaurantEntity);
        return _mapper.Map<RestaurantResponse>(restaurantEntity);
    }

    public async Task<bool> DeleteRestaurantByIdAsync(int restaurantId)
    {
        var existingRestaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (existingRestaurant is null)
        {
            throw new NotFoundException($"The restaurant doesn't exist");
        }
        return await _restaurantRepository.DeleteByIdAsync(restaurantId);
    }

    public async Task UpdateRestaurantByIdAsync(int restaurantId, RestaurantRequest updatedRestaurant)
    {
        if (updatedRestaurant is null)
        {
            throw new ArgumentNullException(nameof(updatedRestaurant));
        }
        var existingRestaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (existingRestaurant is null)
        {
            throw new NotFoundException($"The restaurant doesn't exist");
        }

        _mapper.Map(updatedRestaurant, existingRestaurant);
        await _restaurantRepository.UpdateAsync(existingRestaurant);
    }
    
    public async Task<Decimal> CalculateRestaurantRevenueAsync(int restaurantId)
    {
        return await this._restaurantRepository.CalculateRestaurantRevenueAsync(restaurantId);
    }
    
    public async Task<bool> IsRestaurantExists(int restaurantId)
    {
        return await _restaurantRepository.GetByIdAsync(restaurantId) != null;
    }
}