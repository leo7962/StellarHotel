using StellarHotel.Validations;
using System.ComponentModel.DataAnnotations;

namespace StellarHotel.Dtos;

public class CreateRservationDto
{
    [Required] public int RoomId { get; set; }

    [Required][DataType(DataType.Date)] public DateTime CheckInDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be greater than check-in date.")]
    public DateTime CheckOutDate { get; set; }

    [Range(1, 10, ErrorMessage = "The number of guests must be between 1 and 10.")]
    public int NumberOfGuests { get; set; }

    public bool IncludesBreakfast { get; set; }
}