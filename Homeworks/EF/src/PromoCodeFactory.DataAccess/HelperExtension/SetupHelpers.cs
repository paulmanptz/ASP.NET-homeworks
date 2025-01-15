using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.HelperExtension
{
    public static class SetupHelpers
    {
        public static async Task<int> FillingDatabaseAsync(this AppDbContext context)
        {
            context.AddRange(FakeDataFactory.Employees);
            context.AddRange(FakeDataFactory.Preferences);
            context.AddRange(FakeDataFactory.Customers);

            //var empl = await context.Employees.FirstOrDefaultAsync(x => x.Id == Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"));
            //var cstmr = await context.Customers.FirstOrDefaultAsync(x => x.Id == Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"));
            //var pref = await context.Preferences.FirstOrDefaultAsync(x => x.Id == Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"));
            //var promoCodes = new List<PromoCode>() {

            //    new PromoCode()
            //    {
            //        Code = "123456789",
            //        ServiceInfo = "First promocode",
            //        PartnerName = "123",
            //        PartnerManager = empl,
            //        Customer = cstmr,
            //        Preference = pref
            //    }
            //};
            //context.AddRange(promoCodes);
            
            await context.SaveChangesAsync();

            return 0;
        }

        public static void Clear<T>(this DbContext context) where T : class
        {
            DbSet<T> dbSet = context.Set<T>();
            if (dbSet.Any())
            {
                dbSet.RemoveRange(dbSet.ToList());
            }
        }

    }
}
