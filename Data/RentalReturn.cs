using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Data
{
    public class RentalReturn
    {
        public Guid Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Mileage { get; set; }
        public decimal Price { get; set; }
        
        public RentalBooking RentalBooking { get; set; }
    }
}