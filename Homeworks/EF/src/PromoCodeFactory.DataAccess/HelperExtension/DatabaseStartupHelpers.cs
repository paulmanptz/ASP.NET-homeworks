using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;


namespace PromoCodeFactory.DataAccess.HelperExtension
{
    public static class DatabaseStartupHelpers
    {
        /// <summary>
        /// This makes sure the database is created/updated
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                try
                {
                    await context.Database.MigrateAsync();
                    context.Clear<Role>();
                    context.Clear<Employee>();
                    context.Clear<Customer>();
                    context.Clear<Preference>();
                    context.Clear<CustomerPreference>();
                    context.Clear<PromoCode>();
                    await context.FillingDatabaseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return webHost;
        }
    }
}
