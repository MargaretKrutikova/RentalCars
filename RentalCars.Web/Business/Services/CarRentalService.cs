using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        private IQueryable<Car> FindAvailableCarsForRange(DateTime startDate, DateTime endDate)
            => _context.Cars
                .Where(car => !car.Bookings.Any(booking =>
                    booking.RentalReturn != null &&
                    startDate >= booking.StartDate && startDate <= booking.EndDate ||
                    endDate >= booking.StartDate && endDate <= booking.EndDate ||
                    booking.StartDate >= startDate && booking.EndDate <= endDate));
    }
}