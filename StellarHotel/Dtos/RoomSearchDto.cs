namespace StellarHotel.Dtos;

public class RoomSearchDto
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IncludesBreakfast { get; set; }
    public required string RoomType { get; set; }
}