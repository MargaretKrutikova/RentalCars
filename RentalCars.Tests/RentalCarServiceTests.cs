using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using RentalCars.Web.Api.Controllers;
using RentalCars.Web.Business.Exceptions;
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
            
            context.Bookings.RemoveRange(context.Bookings);
            context.Cars.RemoveRange(context.Cars);
            context.Customers.RemoveRange(context.Customers);
            context.Returns.RemoveRange(context.Returns);
            
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

        private static async Task<RentalBooking> AddBooking(RentalCarsDbContext context, Car car, string startDate, string endDate)
        {
            var customer = context.Customers.First();

            var booking = new RentalBooking
            {
                Id = Guid.NewGuid(),
                CarId = car.Id,
                BookingNumber = "ABC",
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
                CustomerId = customer.Id
            };

            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();
            return booking;
        }
        
        [Fact]
        public async void RentCar_RegistersBooking_For_Available_Car()
        {
            await using var context = new RentalCarsDbContext(ContextOptions);
            
            var service = new CarRentalService(new Mock<IRentalPriceCalculator>().Object, context);
            var firstCar = context.Cars.First();
            var customer = context.Customers.First();

            var startDate = DateTime.Parse("2021-05-05");
            var endDate = DateTime.Parse("2021-06-05");
                
            await service.RentCar(new RentCarModel(firstCar.Id, customer.Id, "ABC", startDate, endDate));

            var booking = context.Bookings.First();
            Assert.Equal(firstCar.Id, booking.CarId);
            Assert.Equal(customer.Id, booking.CustomerId);
            Assert.Equal(startDate, booking.StartDate);
            Assert.Equal(endDate, booking.EndDate);
            Assert.Equal("ABC", booking.BookingNumber);
        }

        [Theory]
        [InlineData("2021-04-01", "2021-04-26")]
        [InlineData("2021-05-01", "2021-05-23")]
        [InlineData("2021-04-13", "2021-05-01")]
        [InlineData("2021-04-10", "2021-05-01")]
        [InlineData("2021-05-10", "2021-05-14")]
        public async void RentCar_ThrowsNonAvailable_IfDateRangeOverlapsWithAnotherBooking(string bookingStartDate, string bookingEndDate)
        {
            await using var context = new RentalCarsDbContext(ContextOptions);

            var service = new CarRentalService(new Mock<IRentalPriceCalculator>().Object, context);
            var firstCar = context.Cars.First();
            var customer = context.Customers.First();

            await AddBooking(context, firstCar, bookingStartDate, bookingEndDate);
            
            var startDate = DateTime.Parse("2021-04-10");
            var endDate = DateTime.Parse("2021-05-10");

            async Task RentCarFn() => 
                await service.RentCar(new RentCarModel(firstCar.Id, customer.Id, "ABC", startDate, endDate));

            await Assert.ThrowsAsync<CarNotAvailable>(RentCarFn);
        }
        
        [Theory]
        [InlineData("2021-04-01", "2021-04-26")]
        [InlineData("2021-05-01", "2021-05-23")]
        [InlineData("2021-04-13", "2021-05-01")]
        [InlineData("2021-04-10", "2021-05-01")]
        [InlineData("2021-05-10", "2021-05-14")]
        public async void FindAvailableCars_ExcludesCars_IfDateRangeOverlapsWithAnotherBooking(string bookingStartDate, string bookingEndDate)
        {
            await using var context = new RentalCarsDbContext(ContextOptions);

            var service = new CarRentalService(new Mock<IRentalPriceCalculator>().Object, context);
            var firstCar = context.Cars.First();

            await AddBooking(context, firstCar, bookingStartDate, bookingEndDate);
            
            var startDate = DateTime.Parse("2021-04-10");
            var endDate = DateTime.Parse("2021-05-10");

            var availableCars =
                await service.FindAvailableCars(firstCar.Category, startDate, endDate);

            Assert.DoesNotContain(availableCars, car => car.Id == firstCar.Id);
        }
        
        [Theory]
        [InlineData("2021-04-01", "2021-04-26")]
        [InlineData("2021-05-10", "2021-05-23")]
        public async void FindAvailableCars_ReturnsCars_AvailableForTheBookingRange(string bookingStartDate, string bookingEndDate)
        {
            await using var context = new RentalCarsDbContext(ContextOptions);

            var service = new CarRentalService(new Mock<IRentalPriceCalculator>().Object, context);
            var firstCar = context.Cars.First();

            await AddBooking(context, firstCar, bookingStartDate, bookingEndDate);
            
            var startDate = DateTime.Parse("2021-04-30");
            var endDate = DateTime.Parse("2021-05-09");

            var availableCars =
                await service.FindAvailableCars(firstCar.Category, startDate, endDate);

            Assert.Single(availableCars);
            Assert.Contains(availableCars, car => car.Id == firstCar.Id);
        }
        
        [Fact]
        public async void ReturnCar_AddsRentalReturn_ForExistingBookings()
        {
            await using var context = new RentalCarsDbContext(ContextOptions);
            
            var service = new CarRentalService(new Mock<IRentalPriceCalculator>().Object, context);
            var customer = context.Customers.First();

            var booking = 
                await AddBooking(context, context.Cars.First(), "2021-05-05", "2021-06-05");

            var returnDate = DateTime.Parse("2021-06-01");
            var rentalReturn = 
                await service.ReturnCar(new ReturnCarModel(booking.BookingNumber, customer.Id, returnDate, 100.0f));

            Assert.True(context.Returns.Contains(rentalReturn));
        }
        
        [Fact]
        public async void ReturnCar_CalculatesReturnPriceCorrectly_ForRentalPeriodAndMileage()
        {
            await using var context = new RentalCarsDbContext(ContextOptions);

            var car = context.Cars.First();
            
            var priceCalculator = new Mock<IRentalPriceCalculator>();
            priceCalculator
                .Setup(x => x.CalculatePrice(car.Category, 12, 89.3f))
                .Returns(345.0m);
            
            var service = new CarRentalService(priceCalculator.Object, context);
            var customer = context.Customers.First();

            var booking = 
                await AddBooking(context, car, "2021-05-05", "2021-06-05");

            var returnDate = DateTime.Parse("2021-05-17");
            var rentalReturn = 
                await service.ReturnCar(new ReturnCarModel(booking.BookingNumber, customer.Id, returnDate, 89.3f));

            Assert.Equal(345.0m, rentalReturn.Price);
        }
    }
}