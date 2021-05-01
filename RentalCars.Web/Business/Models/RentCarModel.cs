using System;

namespace RentalCars.Web.Business.Models
{    
    public record ReturnCarModel(string BookingNumber, Guid CustomerId, DateTime ReturnDate, float Mileage);

    public class RentCarModel
    {
        public Guid CarId { get; init; } 
        public Guid CustomerId { get; init; } 
        public string BookingNumber { get; init; } 
        public DateTime StartDate { get; init; } 
        public DateTime EndDate { get; init; } 
        
        public RentCarModel(Guid carId, Guid customerId, string bookingNumber, DateTime startDate, DateTime endDate)
        {
            CarId = carId;
            CustomerId = customerId;
            BookingNumber = bookingNumber;
            StartDate = startDate;
            EndDate = endDate;
        }

        // public static RentCarModel CreateInstance()
        // {
        //     return new RentCarModel();
        // }

    }
}