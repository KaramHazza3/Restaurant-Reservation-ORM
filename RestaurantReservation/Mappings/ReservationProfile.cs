using AutoMapper;
using RestaurantReservation.Contracts.Requests;
using RestaurantReservation.Contracts.Responses;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Mappings;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<ReservationRequest, Reservation>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) =>
                    srcMember != null && !(srcMember is string s && string.IsNullOrWhiteSpace(s))));
        
        CreateMap<ReservationResponse, Reservation>().ReverseMap();  
    }
}