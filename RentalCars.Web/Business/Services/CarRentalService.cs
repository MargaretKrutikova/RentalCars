using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentalCars.Web.Business.Models;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public class CarRentalService : ICarRentalService
    {
        private readonly RentalCarsDbContext _context;

        public CarRentalService(RentalCarsDbContext context)
        {
            _context = context;
        }

        public Task<List<Car>> FindAvailableCars(CarCategory category, DateTime startDate, DateTime endDate)
            => FindAvailableCarsForRange(startDate, endDate)
                .Where(car => car.Category == category)
                .ToListAsync();

        public async Task ReturnCar(ReturnCarModel model)
        {
            var currentRental = await 
                _context.Bookings.FirstOrDefaultAsync(
                    rental => rental.BookingNumber == model.BookingNumber && rental.CustomerId == model.CustomerId);

            if (currentRental == null)
            {
                throw new Exception("Rental is not found");
            }

            var returnedCar = 
                await _context.Cars.FirstAsync(car => car.Id == currentRental.CarId);

            var returnRental = new RentalReturn()
            {
                Id = Guid.NewGuid(),
                Mileage = model.Mileage,
                RentalBooking = currentRental,
                Price = 100.0m,
                ReturnDate = model.ReturnDate
            };
            
            returnedCar.Mileage += model.Mileage;
            await _context.Returns.AddAsync(returnRental);
            await _context.SaveChangesAsync();
        }

        public async Task RentCar(RentCarModel model)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == model.CustomerId);
            if (customer == null)
            {
                throw new Exception("Customer doesnt exist");
            }

            var carToRent =
                await FindAvailableCarsForRange(model.StartDate, model.EndDate)
                    .FirstOrDefaultAsync(car => car.Id == model.CarId);

            if (carToRent == null)
            {
                throw new Exception("Car can't be found or is not available for the current period");
            }

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
                    booking.RentalReturn != null &&
                    startDate >= booking.StartDate && startDate <= booking.EndDate ||
                    endDate >= booking.StartDate && endDate <= booking.EndDate ||
                    booking.StartDate >= startDate && booking.EndDate <= endDate));
    }
}
