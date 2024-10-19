using StellarHotel.Dtos;

namespace StellarHotel.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> CreateReservationAsync(CreateRservationDto reservationDto);
    Task<ReservationDto> GetReservationByIdAsync(int reservationId);
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<bool> CancelReservationAsync(int reservationId);
}