using System;
using System.Collections.Generic;

namespace RentalCars.Web.Data
{
    public class Car
    {
        public Guid Id { get; set; }
        public CarCategory Category { get; set; }
        public float Mileage { get; set; }
        public string Model { get; set; }
        
        public List<RentalBooking> Bookings { get; set; }
    }
}