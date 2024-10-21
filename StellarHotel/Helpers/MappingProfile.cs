using AutoMapper;
using StellarHotel.Dtos;
using StellarHotel.Models;

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