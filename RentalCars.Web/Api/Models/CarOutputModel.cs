using System;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Models
{
    public class CarOutputModel
    {
        public Guid Id { get; init; }
        public CarCategory Category { get; init; }
        public string Model { get; init; }
        public float Mileage { get; init; }
    }
}