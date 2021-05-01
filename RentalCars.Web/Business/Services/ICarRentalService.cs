using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentalCars.Web.Business.Models;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public interface ICarRentalService
    {
        Task<List<Car>> FindAvailableCars(
            CarCategory category, DateTime startDate, DateTime endDate, DateTime currentDate);
        Task RentCar(RentCarModel model, DateTime currentDate);
        Task<RentalReturn> ReturnCar(ReturnCarModel model);
    }
}