using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace StellarHotel.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;

    public ReservationsController(IReservationService reservationService, IRoomService roomService)
    {
        _reservationService = reservationService;
        _roomService = roomService;
    }

    [HttpGet("available-rooms")]
    [SwaggerOperation(Summary = "Get available rooms",
        Description = "Returns a list of available rooms based on the provided search criteria.")]
    [SwaggerResponse(200, "List of available rooms")]
    [SwaggerResponse(400, "Invalid search criteria")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] RoomSearchDto roomSearchDto)
    {
        try
        {
            var availableRooms = await _roomService.GetAvailableRoomsAsync(roomSearchDto);
            return Ok(availableRooms);
        }
        catch (NpgsqlException ex)
        {
            return StatusCode(500, new { message = "Database connection error: " + ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}", Name = "reservation-details")]
    [SwaggerOperation(Summary = "Get reservation by ID", Description = "Fetches details of a specific reservation.")]
    [SwaggerResponse(200, "Reservation details", typeof(Reservation))]
    [SwaggerResponse(404, "Reservation not found")]
    public async Task<IActionResult> GetReservationById(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null) return NotFound();

        return Ok(reservation);
    }

    [HttpGet("all-reservations")]
    [SwaggerOperation(Summary = "Get all reservations",
        Description = "Returns a list of all reservations categorized as past, ongoing, and future.")]
    [SwaggerResponse(200, "List of reservations", typeof(IEnumerable<Reservation>))]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    [HttpPost("create-reservation")]
    [SwaggerOperation(Summary = "Create a reservation",
        Description = "Creates a new reservation with the provided details.")]
    [SwaggerResponse(201, "Reservation created", typeof(Reservation))]
    [SwaggerResponse(400, "Invalid reservation details")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] CreateRservationDto createRservation)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(createRservation);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Cancel a reservation", Description = "Cancels an existing reservation by ID.")]
    [SwaggerResponse(204, "Reservation cancelled")]
    [SwaggerResponse(404, "Reservation not found")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var success = await _reservationService.CancelReservationAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}