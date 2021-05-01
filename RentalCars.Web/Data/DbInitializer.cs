using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RentalCars.Web.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<RentalCarsDbContext>();
            if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await dbContext.Database.MigrateAsync();
            }

            await dbContext.SaveChangesAsync();
        }
    }
}