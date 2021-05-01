using System;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Models
{
    public class CarOutputModel
    {
        public Guid Id { get; set; }
        public CarCategory Category { get; set; }
        public string Model { get; set; }
        public float Mileage { get; set; }
    }
}