using System;

namespace RentalCars.Web.Business.Models
{
    public record ReturnCarModel(
        string BookingNumber,
        Guid CustomerId,
        DateTime ReturnDate,
        float Mileage);
}