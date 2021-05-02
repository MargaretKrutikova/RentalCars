using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentalCars.Web.Data;

namespace RentalCars.Web.Business.Services
{
    public interface ICustomerService
    {
        Task RegisterCustomer(Customer customer);
        Task<List<RentalBooking>> GerCustomerRentals(Guid customerId);
    }
}