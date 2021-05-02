using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentalCars.Web.Api.Models;
using RentalCars.Web.Business.Exceptions;
using RentalCars.Web.Business.Services;
using RentalCars.Web.Data;

namespace RentalCars.Web.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerInputModel model)
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer {Id = customerId, Email = model.Email, DateOfBirth = model.DateOfBirth};

            try
            {
                await _customerService.RegisterCustomer(customer);
            }
            catch (EmailAlreadyExists)
            {
                return BadRequest("Email already registered");
            }

            return Ok(customerId);
        }
        
        [HttpGet("{customerId}/rentals")]
        public async Task<IActionResult> GetCustomerRentals(Guid customerId)
        {
            try
            {
                var bookings = await _customerService.GerCustomerRentals(customerId);
                var output = new CustomerBookingsOutputModel (bookings.Select(ToBookingOutputModel).ToList());
                
                return Ok(output);
            }
            catch (Exception ex)
            {
                return this.DomainExceptionToResult(ex);
            }
        }

        private static CustomerBookingOutputModel ToBookingOutputModel(RentalBooking booking)
        {
            var returnRentalModel = booking.RentalReturn != null
                ? new BookingReturnOutputModel(booking.RentalReturn.ReturnDate, booking.RentalReturn.Mileage,
                    booking.RentalReturn.Price)
                : null;

            return new CustomerBookingOutputModel(
                booking.BookingNumber, 
                booking.Car.Model, 
                booking.Car.Category,
                booking.StartDate,
                booking.EndDate, 
                returnRentalModel);
        }
    }
}