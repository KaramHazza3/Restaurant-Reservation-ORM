using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class TableProfile : Profile
{
    public TableProfile()
    {
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