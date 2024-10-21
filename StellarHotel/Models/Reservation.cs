using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StellarHotel.Validations;

namespace StellarHotel.Models
{
    public class Reservation
    {
        [Key] public int Id { get; set; }

        [ForeignKey("Room")]
        [Required(ErrorMessage = "The room ID is required.")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "The check-in date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "The check-out date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be greater than check-in date.")]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 10, ErrorMessage = "The number of guests must be between 1 and 10.")]
        public int NumberOfGuests { get; set; }

        public bool IncludesBreakfast { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The total price must be a positive value.")]
        public decimal TotalPrice { get; set; }

        public required Room Room { get; set; }
    }
}
