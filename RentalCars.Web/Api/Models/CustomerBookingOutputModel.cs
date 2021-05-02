#nullable enable
using System;
using System.Collections.Generic;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Models
{
    public record BookingReturnOutputModel(DateTime ReturnDate, float Mileage, decimal Price);

    public record CustomerBookingOutputModel(
        string BookingNumber,
        string CarModel,
        CarCategory CarCategory,
        DateTime BookingStartDate,
        DateTime BookingEndDate,
        BookingReturnOutputModel? ReturnData);

    public record CustomerBookingsOutputModel(List<CustomerBookingOutputModel> Bookings);
}