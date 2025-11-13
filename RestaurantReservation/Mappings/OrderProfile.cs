using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderRequest, Order>()
            .ForMember(dest => dest.ReservationId, opt =>
            {
                opt.PreCondition(src => src.ReservationId.HasValue);
                opt.Condition(src => src.ReservationId.HasValue);
            })
            .ForMember(dest => dest.EmployeeId, opt =>
            {
                opt.PreCondition(src => src.EmployeeId.HasValue);
                opt.Condition(src => src.EmployeeId.HasValue);
            })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));

        CreateMap<OrderResponse, Order>().ReverseMap();
    }
}