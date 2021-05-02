using System;
using Microsoft.EntityFrameworkCore;
using RentalCars.Web.Business.Services;
using RentalCars.Web.Data;
using Xunit;

namespace RentalCars.Tests
{
    public class CustomerServiceTests
    {
        public CustomerServiceTests()
        {
            ContextOptions = new DbContextOptionsBuilder<RentalCarsDbContext>()
                .UseInMemoryDatabase("RentalCars")
                .Options;
            Seed();
        }

        private DbContextOptions<RentalCarsDbContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new RentalCarsDbContext(ContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }

        [Fact]
        public async void RegisterCustomer_SavesCustomerInfo()
        {
            await using var context = new RentalCarsDbContext(ContextOptions);
            
            var service = new CustomerService(context);
            var customer = new Customer()
                {Id = Guid.NewGuid(), Email = "abc@abc.com", DateOfBirth = DateTime.Parse("1980-05-05")};
            
            await service.RegisterCustomer(customer);

            var registeredCustomer = 
                await context.Customers.SingleAsync(c => c.Id == customer.Id);
            
            Assert.Equal(customer.DateOfBirth, registeredCustomer.DateOfBirth);
            Assert.Equal(customer.Email, registeredCustomer.Email);
        }
    }
}