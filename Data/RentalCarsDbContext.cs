using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Data
{
    public class RentalCarsDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<RentalBooking> Bookings { get; set; }
        public DbSet<RentalReturn> Returns { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        public RentalCarsDbContext(DbContextOptions<RentalCarsDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Car>()
                .Property(e => e.Category)
                .HasConversion(
                    category => category.ToString(),
                    value => (CarCategory) Enum.Parse(typeof(CarCategory), value));
                
            var cars = new Car[]
            {
                new() {Id = Guid.NewGuid(), Category = CarCategory.Compact, Model = "Zaporozhets"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Premium, Model = "Lamborghini Huracán"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Minivan, Model = "Volvo"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Premium, Model = "Porsche Panamera"},
                new() {Id = Guid.NewGuid(), Category = CarCategory.Compact, Model = "Lada Riva"},
            };
            var customers = new Customer[]
            {
                new()
                {
                    Id = Guid.NewGuid(), Email = "test",
                    DateOfBirth = DateTime.Parse("1990-01-01")
                },
                new()
                {
                    Id = Guid.NewGuid(), Email = "test2",
                    DateOfBirth = DateTime.Parse("1990-01-01")
                },
            };
            modelBuilder.Entity<Car>().HasData(cars);
            modelBuilder.Entity<Customer>().HasData(customers);
            
            modelBuilder.Entity<RentalBooking>().HasData(new RentalBooking[] {
                new()
                {
                    Id = Guid.NewGuid(), 
                    BookingNumber = "ABC12",
                    CustomerId = customers.First().Id,
                    CarId = cars.First().Id,
                    StartDate = DateTime.Parse("2021-06-01"),
                    EndDate = DateTime.Parse("2021-06-24")
                },
                new()
                {
                    Id = Guid.NewGuid(), 
                    BookingNumber = "ABC13",
                    CustomerId = customers.Skip(1).First().Id,
                    CarId = cars.First().Id,
                    StartDate = DateTime.Parse("2021-07-12"),
                    EndDate = DateTime.Parse("2021-07-16")
                }
            });
        }
    }
}