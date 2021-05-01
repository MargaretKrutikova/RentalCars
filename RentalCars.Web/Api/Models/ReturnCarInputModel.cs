using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Web.Api.Models
{
    public class ReturnCarInputModel
    {
        [Required]
        public string BookingNumber { get; init; } 
    
        [Required]
        public string CustomerEmail { get; init; } 
    
        [Required]
        public float Mileage { get; init; } 
    }
}