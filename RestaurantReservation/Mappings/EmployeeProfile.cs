using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
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
    }
}