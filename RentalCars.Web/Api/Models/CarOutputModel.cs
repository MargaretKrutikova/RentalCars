using System;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Models
{
    public record CarOutputModel(Guid Id, CarCategory Category, string Model, float Mileage);
}