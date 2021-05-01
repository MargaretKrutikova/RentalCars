using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RentalCars.Web.Api.Controllers;
using RentalCars.Web.Business.Models;
using RentalCars.Web.Business.Services;
using RentalCars.Web.Data;
using Xunit;

namespace RentalCars.Tests
{
    public class RentalCarServiceTests
    {
        public RentalCarServiceTests()
        {
            ContextOptions = new DbContextOptionsBuilder<RentalCarsDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            Seed();
        }

        private DbContextOptions<RentalCarsDbContext> ContextOptions { get; }
        
        private void Seed()
        {
            using var context = new RentalCarsDbContext(ContextOptions);
            
            var cars = new Car[]
            {
                new() {Id = Guid.NewGuid(), Category = CarCategory.Compact, Model = "Zaporozhets"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Premium, Model = "Lamborghini Huracán"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Minivan, Model = "Volvo"},
            };
            var customers = new Customer[]
                {new() {Id = Guid.NewGuid(), Email = "test", DateOfBirth = DateTime.Parse("1990-01-01")}};

            context.Cars.AddRange(cars);
            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
        
        [Fact]
        public async void RentCar_RegistersBooking_For_Available_Car()
        {
            await using var context = new RentalCarsDbContext(ContextOptions);
            
            var service = new CarRentalService(context);
            var firstCar = context.Cars.First();
            var customer = context.Customers.First();

            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(10);
                
            await service.RentCar(new RentCarModel(firstCar.Id, customer.Id, "ABC", startDate, endDate));

            var booking = context.Bookings.First();
            Assert.Equal(firstCar.Id, booking.CarId);
            Assert.Equal(customer.Id, booking.CustomerId);
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal("ABC", booking.BookingNumber);
        }
    }
}