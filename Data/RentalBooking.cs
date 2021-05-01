using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Data
{
    public class RentalBooking
    {
        public Guid Id { get; set; }
        
        [Required]
        public string BookingNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
        public Guid? RentalReturnId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;  }
        
        public Car Car { get; set; }
        public Customer Customer { get; set; }
        public RentalReturn RentalReturn { get; set; }
    }
}