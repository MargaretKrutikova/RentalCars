using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalCars.Web.Api.Models;
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

        [HttpPost("rent")]
        public async Task<IActionResult> RentCar(RentCarInputModel inputModel)
        {
            var bookingNumber = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            try
            {
                await _carRentalService.RentCar(new RentCarModel(inputModel.CarId, inputModel.CustomerId, bookingNumber,
                    inputModel.StartDate, inputModel.EndDate));
            }
            catch (Exception ex)
            {
                return this.DomainExceptionToResult(ex);
            }
            
            return NoContent();
        }
        
        [HttpPost("return")]
        public async Task<IActionResult> Return(ReturnCarInputModel inputModel)
        {
            var returnDate = new DateTime();

            try
            {
                var rentalReturn = await _carRentalService.ReturnCar(
                    new ReturnCarModel(inputModel.BookingNumber, inputModel.CustomerId, returnDate, inputModel.Mileage));
                
                return Ok(new ReturnCarOutputModel {Price = rentalReturn.Price});
            }
            catch (Exception ex)
            {
                return this.DomainExceptionToResult(ex);
            }
        }
    }
}
