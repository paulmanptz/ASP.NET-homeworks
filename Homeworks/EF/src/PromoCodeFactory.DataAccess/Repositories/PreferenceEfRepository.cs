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

    public class PreferenceEfRepository : IRepository<Preference>
    {
        protected AppDbContext _context;

        public PreferenceEfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Preference>> GetAllAsync()
        {
            var entitySet = _context.Preferences;
            return await entitySet.ToListAsync();
        }

        public Task<Preference> GetByIdAsync(Guid id)
        //async Task<Preference> IRepository<Preference>.GetByIdAsync(Guid id)
        {
            //return await _context.Preferences.FirstAsync(x => x.Id == id);
            //return _context.Preferences.FirstAsync(x => x.Id == id);

            var pref = _context.Preferences.Find(id);
            //return _context.Preferences.FindAsync(x => x.Id == id);
            return Task.FromResult(pref);
        }
    }

}