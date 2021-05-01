using System;

namespace RentalCars.Web.Business.Models
{
    public record ReturnCarModel(
        string BookingNumber,
        string CustomerEmail,
        DateTime ReturnDate,
        float Mileage);
}