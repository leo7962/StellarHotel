using System.ComponentModel.DataAnnotations;

namespace StellarHotel.Models
{
    public class Room
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "The room type is required.")]
        [StringLength(50, ErrorMessage = "The room type cannot exceed 50 characters.")]
        public required string Type { get; set; }

        [Range(1, 10, ErrorMessage = "The maximum occupancy must be between 1 and 10.")]
        public int MaxOccupancy { get; set; }

        [Range(1, 5, ErrorMessage = "The number of beds must be between 1 and 5.")]
        public int NumberOfBeds { get; set; }

        public bool HasOceanView { get; set; }

        [Range(1, 1000, ErrorMessage = "The base rate must be between $1 and $1000.")]
        public decimal BaseRate { get; set; }
    }
}
