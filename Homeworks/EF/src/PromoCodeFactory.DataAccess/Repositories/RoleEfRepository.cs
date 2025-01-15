using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{

    public class RoleEfRepository : IRepository<Role>
    {
        protected AppDbContext _context;

        public RoleEfRepository(AppDbContext context)
        {
            _context = context; 
        }

        async Task<IEnumerable<Role>> IRepository<Role>.GetAllAsync()
        {
            var entitySet = _context.Roles;
            return await entitySet.ToListAsync();
        }

        Task<Role> IRepository<Role>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
