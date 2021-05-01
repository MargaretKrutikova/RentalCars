using System;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public class RentalPriceCalculator : IRentalPriceCalculator
    {
        private const decimal BaseDayRental = 100.0m;
        private const decimal KilometerPrice = 10.0m;

        public decimal CalculatePrice(CarCategory carCategory, int numberOfDays, float numberOfKilometers) =>
            carCategory switch
            {
                CarCategory.Compact => numberOfDays * BaseDayRental,
                CarCategory.Minivan => CalculateMinivanPrice(numberOfDays, numberOfKilometers),
                CarCategory.Premium => CalculatePremiumPrice(numberOfDays, numberOfKilometers),
                _ => throw new ArgumentOutOfRangeException(nameof(carCategory), carCategory, null)
            };

        private decimal CalculateMinivanPrice(int numberOfDays, float numberOfKilometers)
            => numberOfDays * BaseDayRental * 1.2m + (decimal) numberOfKilometers * KilometerPrice;

        private decimal CalculatePremiumPrice(int numberOfDays, float numberOfKilometers)
            => numberOfDays * BaseDayRental * 1.7m +  (decimal) numberOfKilometers * KilometerPrice * 1.5m;
    }
}