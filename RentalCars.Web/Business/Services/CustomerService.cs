using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            {
                throw new Exception("Email already exists");
            }
            await _context.Customers.AddAsync(customer);
        }
    }
}