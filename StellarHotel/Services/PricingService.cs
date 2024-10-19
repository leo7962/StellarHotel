using StellarHotel.Context;
using StellarHotel.Dtos;
using StellarHotel.Interfaces;

namespace StellarHotel.Services;

public class PricingService : IPricingService
{
    private readonly DataContext _context;

    public PricingService(DataContext context)
    {
        _context = context;
    }

    public async Task<decimal> CalculatePriceAsync(PricingRequestDto pricingRequest)
    {
        var room = await _context.Rooms.FindAsync(pricingRequest.Id);
        if (room == null) throw new Exception("Room not found.");

        var totalDays = (pricingRequest.CheckOutDate - pricingRequest.CheckInDate).Days;
        decimal totalPrice = 0;

        for (var date = pricingRequest.CheckInDate; date < pricingRequest.CheckOutDate; date = date.AddDays(1))
        {
            var dailyRate = room.BaseRate;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) dailyRate *= 1.25m;

            if (totalDays >= 4 && totalDays <= 6)
                dailyRate -= 4;
            else if (totalDays >= 7 && totalDays <= 9)
                dailyRate -= 8;
            else if (totalDays >= 10) dailyRate -= 12;

            totalPrice += dailyRate;
        }

        if (pricingRequest.IncludesBreakfast)
        {
            var breakfastCost = GetBreakfastCostAsync(pricingRequest.NumberOfGuests, totalDays);
            totalPrice += breakfastCost;
        }

        return totalPrice;
    }

    private decimal GetBreakfastCostAsync(int numberOfGuests, int totalDays)
    {
        var breakfastPricePerDay = 5m;

        return numberOfGuests * totalDays * breakfastPricePerDay;
    }
}