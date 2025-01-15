using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{

    public class CustomerEfRepository : IExpandedRepository<Customer>
    {
        protected AppDbContext _context;

        public CustomerEfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var entitySet = _context.Customers.Include(item => item.CustomerPreferences);
            return await entitySet.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var customrs = _context.Customers
                .Include(u => u.Promocodes);

            return await customrs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateNewRecordAsync(Customer entity)
        {
            await _context.Set<Customer>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecordAsync(Customer customer)
        {
            var entity = await _context.Customers.Include(item => item.CustomerPreferences).FirstOrDefaultAsync(item => item.Id == customer.Id);
            if (entity != null)
            {
                entity.FirstName = customer.FirstName;
                entity.LastName = customer.LastName;
                entity.Email = customer.Email;
                entity.CustomerPreferences.Clear();
                entity.CustomerPreferences.AddRange(customer.CustomerPreferences);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecordAsync(Customer entity)
        {
            _context.Set<Customer>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        Task IExpandedRepository<Customer>.DeleteRecordsAsync(List<Customer> entities)
        {
            throw new NotImplementedException();
        }
    }
}
