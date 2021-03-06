using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Web.Api.Models
{
    public class RentCarInputModel
    {
        [Required]
        public Guid CarId { get; init; } 
        
        [Required]
        public string CustomerEmail { get; init; } 
        
        [Required]
        public DateTime StartDate { get; init; } 
        
        [Required]
        public DateTime EndDate { get; init; } 
    }
}