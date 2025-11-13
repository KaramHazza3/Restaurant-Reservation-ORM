using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItemRequest, OrderItem>()
            .ForMember(dest => dest.MenuItemId,
                opt =>
                {
                    opt.PreCondition(src => src.MenuItemId.HasValue);
                    opt.Condition(src => src.MenuItemId.HasValue);
                })
            .ForMember(dest => dest.OrderId,
                opt =>
                {
                    opt.PreCondition(src => src.OrderId.HasValue);
                    opt.Condition(src => src.OrderId.HasValue);
                })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<OrderItemResponse, OrderItem>().ReverseMap();
    }
}