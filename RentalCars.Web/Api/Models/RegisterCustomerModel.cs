using System;
using System.ComponentModel.DataAnnotations;

namespace RentalCars.Web.Api.Models
{
    public class RegisterCustomerInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        
        [Required]
        public DateTime DateOfBirth { get; init; }
    }
}