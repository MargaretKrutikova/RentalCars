using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Web.Api.Models
{
    public class ReturnCarInputModel
    {
        [Required]
        public string BookingNumber { get; init; } 
    
        [Required]
        public Guid CustomerId { get; init; } 
    
        [Required]
        public float Mileage { get; init; } 
    }
}