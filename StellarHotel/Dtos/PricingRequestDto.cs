namespace StellarHotel.Dtos;

public class PricingRequestDto
{
    public int Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IncludesBreakfast { get; set; }
}