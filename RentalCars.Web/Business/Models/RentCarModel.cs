using System;

namespace RentalCars.Web.Business.Models
{
    public record RentCarModel(
        Guid BookingId,
        Guid CarId,
        string CustomerEmail,
        string BookingNumber,
        DateTime StartDate,
        DateTime EndDate);
}