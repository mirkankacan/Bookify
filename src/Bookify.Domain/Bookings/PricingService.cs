using Bookify.Domain.Apartments;

namespace Bookify.Domain.Bookings
{
    public class PricingService
    {
        public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
        {
            var currency = apartment.Price.Currency;
            var priceForPeriod = new Money(apartment.Price.Amount * period.LengthInDays, currency);
            decimal percentangeUpCharge = 0;
            foreach (var amenity in apartment.Amenities)
            {
                percentangeUpCharge += amenity switch
                {
                    Amenity.GardenView or Amenity.MountainView => 0.05m,
                    Amenity.AirConditioning => 0.10m,
                    Amenity.Parking => 0.15m,
                    _ => 0
                };
            }
            var amenitiesUpCharge = Money.Zero();
            if (percentangeUpCharge > 0)
            {
                amenitiesUpCharge = new Money(priceForPeriod.Amount * percentangeUpCharge, currency);
            }
            var totalPrice = Money.Zero();
            totalPrice += priceForPeriod;
            if (!apartment.CleaningFee.IsZero())
            {
                totalPrice += apartment.CleaningFee;
            }
            return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
        }
    }
}