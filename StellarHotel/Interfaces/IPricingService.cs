using StellarHotel.Dtos;

namespace StellarHotel.Interfaces;

public interface IPricingService
{
    Task<decimal> CalculatePriceAsync(PricingRequestDto pricingRequest);
}