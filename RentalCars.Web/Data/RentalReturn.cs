using System;

namespace RentalCars.Web.Data
{
    public class RentalReturn
    {
        public Guid Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public float Mileage { get; set; }
        public decimal Price { get; set; }
        
        public RentalBooking RentalBooking { get; set; }
    }
}