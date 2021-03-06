using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Web.Data
{
    public class Customer
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}