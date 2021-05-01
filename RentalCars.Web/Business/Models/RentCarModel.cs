using System;

namespace RentalCars.Web.Business.Models
{
    public record RentCarModel(
        Guid BookingId,
        Guid CarId,
        Guid CustomerId,
        string BookingNumber,
        DateTime StartDate,
        DateTime EndDate);
}