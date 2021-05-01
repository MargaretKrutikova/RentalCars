using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalCars.Web.Api.Models;
using RentalCars.Web.Business;
using RentalCars.Web.Business.Models;
using RentalCars.Web.Business.Services;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Controllers
{
    [ApiController]
    [Route("rentals")]
    public class CarRentalController : ControllerBase
    {
        private readonly ICarRentalService _carRentalService;

        public CarRentalController(ICarRentalService carRentalService)
        {
            _carRentalService = carRentalService;
        }

        [HttpGet("available")]
        public async Task<IEnumerable<CarOutputModel>> GetAvailableCars(
            CarCategory category,
            DateTime startDate,
            DateTime endDate)
        {
            var currentDate = DateTime.Now;
            var cars = await _carRentalService.FindAvailableCars(category, startDate, endDate, currentDate);

            return cars.Select(
                car => new CarOutputModel(car.Id, car.Category, car.Model, car.Mileage));
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentCar(RentCarInputModel model)
        {
            var currentDate = DateTime.Now;
            var bookingNumber = Utils.GenerateBookingNumber();

            try
            {
                await _carRentalService.RentCar(
                    new RentCarModel(
                        Guid.NewGuid(), model.CarId, model.CustomerEmail, bookingNumber, model.StartDate, model.EndDate),
                    currentDate);
            }
            catch (Exception ex)
            {
                return this.DomainExceptionToResult(ex);
            }

            return Ok(new RentCarOutputModel(bookingNumber));
        }
        
        [HttpPost("return")]
        public async Task<IActionResult> Return(ReturnCarInputModel model)
        {
            var returnDate = DateTime.Now;

            try
            {
                var rentalReturn = await _carRentalService.ReturnCar(
                    new ReturnCarModel(model.BookingNumber, model.CustomerId, returnDate, model.Mileage));
                
                return Ok(new ReturnCarOutputModel(rentalReturn.Price));
            }
            catch (Exception ex)
            {
                return this.DomainExceptionToResult(ex);
            }
        }
    }
}
