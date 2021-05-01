using System;
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
    }
}