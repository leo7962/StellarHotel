using AutoMapper;
using Core.Models;
using StellarHotel.Dtos;

namespace StellarHotel.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Room, RoomDto>().ReverseMap();
        CreateMap<CreateRservationDto, Reservation>().ReverseMap();
        CreateMap<Reservation, ReservationDto>().ReverseMap();
        CreateMap<RoomSearchDto, Room>().ReverseMap();
    }
}