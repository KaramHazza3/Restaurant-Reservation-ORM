using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<RestaurantRequest, Restaurant>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<RestaurantResponse, Restaurant>().ReverseMap();
    }
}