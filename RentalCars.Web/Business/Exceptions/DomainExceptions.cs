using System;

namespace RentalCars.Web.Business.Exceptions
{
    public class EmailAlreadyExists : Exception { }
    public class CustomerNotFound : Exception {}
    public class BookingNotFound : Exception {}
    public class CarNotAvailable : Exception {}
}