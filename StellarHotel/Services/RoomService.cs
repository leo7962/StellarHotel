using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StellarHotel.Context;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;

namespace StellarHotel.Services;

public class RoomService : IRoomService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RoomService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(RoomSearchDto searchDto)
    {
        var availableRooms = await _context.Rooms.Where(r => r.MaxOccupancy >= searchDto.NumberOfGuests).ToListAsync();

        var unavailableRoomIds = await _context.Reservations.Where(res =>
                res.CheckInDate < searchDto.CheckOutDate && res.CheckOutDate > searchDto.CheckInDate)
            .Select(res => res.RoomId).ToListAsync();

        availableRooms = availableRooms.Where(r => !unavailableRoomIds.Contains(r.Id)).ToList();

        if (!string.IsNullOrEmpty(searchDto.RoomType))
            availableRooms = availableRooms
                .Where(r => r.Type.Equals(searchDto.RoomType, StringComparison.OrdinalIgnoreCase)).ToList();

        return _mapper.Map<IEnumerable<RoomDto>>(availableRooms);
    }

    public async Task<RoomDto> GetRoomByIdAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        return _mapper.Map<RoomDto>(room);
    }
}