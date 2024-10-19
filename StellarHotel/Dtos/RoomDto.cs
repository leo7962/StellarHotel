namespace StellarHotel.Dtos;

public class RoomDto
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public int MaxOccupancy { get; set; }
    public int NumberOfBeds { get; set; }
    public bool HasOceanView { get; set; }
    public decimal BaseRate { get; set; }
}