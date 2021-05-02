using System;
using Microsoft.EntityFrameworkCore;

namespace RentalCars.Web.Data
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
                    Id = Guid.Parse("2c1366e2-f73c-4aee-8608-d510c27a9ad5"), Email = "test@testsson.com",
                    DateOfBirth = DateTime.Parse("1990-01-01")
                }
            };
            modelBuilder.Entity<Car>().HasData(cars);
            modelBuilder.Entity<Customer>().HasData(customers);
        }
    }
}