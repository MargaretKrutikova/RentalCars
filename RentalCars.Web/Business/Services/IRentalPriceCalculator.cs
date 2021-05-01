using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public interface IRentalPriceCalculator
    {
        decimal CalculatePrice(CarCategory carCategory, int numberOfDays, float numberOfKilometers);
    }
}