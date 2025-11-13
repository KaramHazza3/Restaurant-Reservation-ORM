using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class MenuItemProfile : Profile
{
    public MenuItemProfile()
    {
        CreateMap<MenuItemRequest, MenuItem>()
            .ForMember(dest => dest.RestaurantId,
                opt =>
                {
                    opt.PreCondition(src => src.RestaurantId.HasValue);
                    opt.Condition(src => src.RestaurantId.HasValue);
                })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<MenuItemResponse, MenuItem>().ReverseMap();
    }
}