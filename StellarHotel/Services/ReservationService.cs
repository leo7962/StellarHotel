using AutoMapper;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using StellarHotel.Context;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;

namespace StellarHotel.Services;

public class ReservationService : IReservationService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IPricingService _pricingService;

    public ReservationService(DataContext context, IMapper mapper, IPricingService pricingService)
    {
        _context = context;
        _mapper = mapper;
        _pricingService = pricingService;
    }

    public async Task<bool> CancelReservationAsync(int reservationId)
    {
        var reservation = await _context.Reservations.FindAsync(reservationId);

        if (reservation == null) return false;

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateRservationDto reservationDto)
    {
        var room = await _context.Rooms.FindAsync(reservationDto.RoomId);
        if (room == null) throw new Exception("Room not found.");

        var pricingRequest = new PricingRequestDto
        {
            Id = reservationDto.RoomId,
            CheckInDate = reservationDto.CheckInDate,
            CheckOutDate = reservationDto.CheckOutDate,
            NumberOfGuests = reservationDto.NumberOfGuests,
            IncludesBreakfast = reservationDto.IncludesBreakfast
        };

        var totalPrice = await _pricingService.CalculatePriceAsync(pricingRequest);

        var reservation = _mapper.Map<Reservation>(reservationDto);
        reservation.TotalPrice = totalPrice;

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto> GetReservationByIdAsync(int reservationId)
    {
        var reservation =
            await _context.Reservations.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == reservationId);

        return _mapper.Map<ReservationDto>(reservation);
    }
}