using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentalCars.Web.Api.Models;
using RentalCars.Web.Business;
using RentalCars.Web.Business.Services;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Controllers
{
    [ApiController]
    [Route("rentals")]
    public class CarRentalController : ControllerBase
    {
        private readonly ILogger<CarRentalController> _logger;
        private readonly ICarRentalService _carRentalService;

        public CarRentalController(ILogger<CarRentalController> logger, ICarRentalService carRentalService)
        {
            _logger = logger;
            _carRentalService = carRentalService;
        }

        [HttpGet("available")]
        public async Task<IEnumerable<CarOutputModel>> GetAvailableCars(CarCategory category, DateTime startDate, DateTime endDate)
        {
            var cars = await _carRentalService.FindAvailableCars(category, startDate, endDate);
            return cars.Select(
                car => new CarOutputModel()
                {
                    Id = car.Id, 
                    Category = car.Category, 
                    Mileage = car.Mileage,
                    Model = car.Model
                });
        }
    }
}