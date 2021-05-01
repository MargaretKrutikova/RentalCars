using System;

namespace RentalCars.Web.Business
{
    public static class Utils
    {
        public static string GenerateBookingNumber()
            => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}