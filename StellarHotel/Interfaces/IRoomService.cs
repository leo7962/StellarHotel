using StellarHotel.Dtos;

namespace StellarHotel.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(RoomSearchDto searchDto);
    Task<RoomDto> GetRoomByIdAsync(int id);
}