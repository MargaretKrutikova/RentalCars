using System;
using Microsoft.AspNetCore.Mvc;
using RentalCars.Web.Business.Exceptions;

namespace RentalCars.Web.Api
{
    public static class HttpUtils
    {
        public static IActionResult DomainExceptionToResult(this ControllerBase controller, Exception ex) =>
            ex switch
            {
                CustomerNotFound => controller.NotFound("Customer not found."),
                BookingNotFound => controller.NotFound("Booking not found."),
                EmailAlreadyExists => controller.BadRequest("Email already registered"),
                CarNotAvailable => controller.BadRequest("Car is not available"),
                DateRangeInvalid => controller.BadRequest("Date range is invalid"),
                _ => controller.StatusCode(500)
            };
    }
}