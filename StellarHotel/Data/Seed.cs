using System.Text.Json;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using StellarHotel.Context;

namespace StellarHotel.Data;

public class Seed
{
    public static async Task SeedRooms(DataContext context)
    {
        if (await context.Rooms.AnyAsync()) return;

        var roomData = await File.ReadAllTextAsync("Data\\RoomSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var rooms = JsonSerializer.Deserialize<List<Room>>(roomData, options);

        if (rooms == null) return;

        foreach (var room in rooms) await context.Rooms.AddAsync(room);

        await context.SaveChangesAsync();
    }
}