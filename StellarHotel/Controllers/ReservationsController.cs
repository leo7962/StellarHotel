using Microsoft.AspNetCore.Mvc;
using Npgsql;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;
using StellarHotel.Models;
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

    /// <summary>
    /// Gets a list of available rooms for a range of dates.
    /// </summary>
    /// <param name="roomSearchDto">Parámetros de búsqueda, incluyendo fechas y número de huéspedes.</param>
    /// <returns>List of available rooms with price details.</returns>
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

    /// <summary>
    /// Gets the details of a specific reservation.
    /// </summary>
    /// <param name="id">Reserve ID.</param>
    /// <returns>Reservation details.</returns>
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

    /// <summary>
    /// Obtains a list of all reservations.
    /// </summary>
    /// <returns>List of past, current, and future reservations.</returns>
    [HttpGet("all-reservations")]
    [SwaggerOperation(Summary = "Get all reservations",
        Description = "Returns a list of all reservations categorized as past, ongoing, and future.")]
    [SwaggerResponse(200, "List of reservations", typeof(IEnumerable<Reservation>))]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// Create a new reservation.
    /// </summary>
    /// <param name="createRservation">Details of the reserve to be created.</param>
    /// <returns>The reserve created.</returns>
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

    /// <summary>
    /// Cancels an existing reservation.
    /// </summary>
    /// <param name="id">ID of the reservation to be cancelled.</param>
    /// <returns>NoContent if the cancellation was successful.</returns>
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