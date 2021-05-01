using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCars.Web.Business.Exceptions;
using RentalCars.Web.Business.Models;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public class CarRentalService : ICarRentalService
    {
        private readonly IRentalPriceCalculator _priceCalculator;
        private readonly RentalCarsDbContext _context;

        public CarRentalService(
            IRentalPriceCalculator priceCalculator,
            RentalCarsDbContext context)
        {
            _priceCalculator = priceCalculator;
            _context = context;
        }

        public Task<List<Car>> FindAvailableCars(CarCategory category, DateTime startDate, DateTime endDate)
            => FindAvailableCarsForRange(startDate, endDate)
                .Where(car => car.Category == category)
                .ToListAsync();

        public async Task<RentalReturn> ReturnCar(ReturnCarModel model)
        {
            var booking = await 
                _context.Bookings.FirstOrDefaultAsync(
                    rental => rental.BookingNumber == model.BookingNumber && rental.CustomerId == model.CustomerId);

            if (booking == null)
                throw new BookingNotFound();

            var returnedCar = 
                await _context.Cars.FirstAsync(car => car.Id == booking.CarId);

            var rentalDays = (model.ReturnDate - booking.StartDate).Days;
            var price = _priceCalculator.CalculatePrice(returnedCar.Category, rentalDays, model.Mileage);
            
            var rentalReturn = new RentalReturn
            {
                Id = booking.Id,
                Mileage = model.Mileage,
                RentalBooking = booking,
                Price = price,
                ReturnDate = model.ReturnDate
            };
            await _context.Returns.AddAsync(rentalReturn);

            returnedCar.Mileage += model.Mileage;
            await _context.SaveChangesAsync();
            
            return rentalReturn;
        }

        public async Task RentCar(RentCarModel model)
        {
            var customer = 
                await _context.Customers.FirstOrDefaultAsync(c => c.Id == model.CustomerId);
            
            if (customer == null)
                throw new CustomerNotFound();

            var carToRent =
                await FindAvailableCarsForRange(model.StartDate, model.EndDate)
                    .FirstOrDefaultAsync(car => car.Id == model.CarId);

            if (carToRent == null)
                throw new CarNotAvailable();

            var booking = new RentalBooking()
            {
                Id = Guid.NewGuid(),
                Car = carToRent,
                BookingNumber = model.BookingNumber,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Customer = customer
            };
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }
        
        private IQueryable<Car> FindAvailableCarsForRange(DateTime startDate, DateTime endDate)
            => _context.Cars
                .Where(car => !car.Bookings.Any(booking =>
                    booking.RentalReturn == null &&
                    (startDate >= booking.StartDate && startDate <= booking.EndDate ||
                     endDate >= booking.StartDate && endDate <= booking.EndDate ||
                     booking.StartDate >= startDate && booking.EndDate <= endDate))
                );
    }
}
