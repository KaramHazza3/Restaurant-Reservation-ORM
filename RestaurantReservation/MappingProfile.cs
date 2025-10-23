using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerRequest, Customer>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<CustomerResponse, Customer>().ReverseMap();

        CreateMap<EmployeeRequest, Employee>()
            .ForMember(dest => dest.RestaurantId,
                opt =>
                {
                    opt.PreCondition(src => src.RestaurantId.HasValue);
                    opt.Condition(src => src.RestaurantId.HasValue);
                })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<EmployeeResponse, Employee>().ReverseMap();
        
        CreateMap<OrderRequest, Order>()
            .ForMember(dest => dest.ReservationId,
                opt =>
                {
                    opt.PreCondition(src => src.ReservationId.HasValue);
                    opt.Condition(src => src.ReservationId.HasValue);
                })
            .ForMember(dest => dest.EmployeeId,
                opt =>
                {
                    opt.PreCondition(src => src.EmployeeId.HasValue);
                    opt.Condition(src => src.EmployeeId.HasValue);
                })
            .ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) =>
                srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<OrderResponse, Order>().ReverseMap();
        
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
        
        CreateMap<ReservationRequest, Reservation>()
            .ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) =>
                srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<ReservationResponse, Reservation>().ReverseMap();
        
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
        
        CreateMap<RestaurantRequest, Restaurant>()
            .ForAllMembers(opt =>
            opt.Condition((src, dest, srcMember) =>
                srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<RestaurantResponse, Restaurant>().ReverseMap();
        
        CreateMap<TableRequest, Table>()
            .ForMember(dest => dest.TableNumber,
                opt =>
                {
                    opt.PreCondition(src => src.RestaurantId.HasValue);
                    opt.Condition(src => src.RestaurantId.HasValue);
                })
            .ForMember(dest => dest.RestaurantId,
                opt =>
                {
                    opt.PreCondition(src => src.RestaurantId.HasValue);
                    opt.Condition(src => src.RestaurantId.HasValue);
                })
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));

        CreateMap<TableResponse, Table>().ReverseMap();
    }
}