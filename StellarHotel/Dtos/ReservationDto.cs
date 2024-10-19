namespace StellarHotel.Dtos;

public class ReservationDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public bool IncludesBreakfast { get; set; }
    public decimal TotalPrice { get; set; }
}