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
    public class PromoCodeEfRepository : IExpandedRepository<PromoCode>
    {
        protected AppDbContext _context;

        public PromoCodeEfRepository(AppDbContext context)
        {
            _context = context;
        }


        async Task IExpandedRepository<PromoCode>.CreateNewRecordAsync(PromoCode entity)
        {
            await _context.Set<PromoCode>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecordAsync(PromoCode entity)
        {
            _context.Set<PromoCode>().Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteRecordsAsync(List<PromoCode> entities)
        {
            _context.Set<PromoCode>().RemoveRange(entities);
            await _context.SaveChangesAsync();

        }

        async Task<IEnumerable<PromoCode>> IRepository<PromoCode>.GetAllAsync()
        {
            var entitySet = _context.PromoCodes;
            return await entitySet.ToListAsync();

        }

        async Task<PromoCode> IRepository<PromoCode>.GetByIdAsync(Guid id)
        {
            var empls = _context.PromoCodes;

            return await empls.FirstAsync(x => x.Id == id);

        }

        Task IExpandedRepository<PromoCode>.UpdateRecordAsync(PromoCode entity)
        {
            throw new NotImplementedException();
        }
    }

}
