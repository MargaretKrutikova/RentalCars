using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCars.Web.Business.Exceptions;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly RentalCarsDbContext _context;

        public CustomerService(RentalCarsDbContext context)
        {
            _context = context;
        }

        public async Task RegisterCustomer(Customer customer)
        {
            var isUniqueCustomer =
                !await _context.Customers.AnyAsync(c => c.Email == customer.Email);

            if (!isUniqueCustomer)
                throw new EmailAlreadyExists();

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RentalBooking>> GerCustomerRentals(Guid customerId)
            => await _context.Bookings
                .Include(booking => booking.RentalReturn)
                .Include(booking => booking.Car)
                .Where(booking => booking.CustomerId == customerId)
                .ToListAsync();
    }
}