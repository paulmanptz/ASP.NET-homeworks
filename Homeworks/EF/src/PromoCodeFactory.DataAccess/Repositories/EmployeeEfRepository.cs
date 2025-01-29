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

    public class EmployeeEfRepository : IRepository<Employee>
    {
        protected AppDbContext _context;

        public EmployeeEfRepository(AppDbContext context)
        {
            _context = context; 
        }
            
        async Task<IEnumerable<Employee>> IRepository<Employee>.GetAllAsync()
        {
            var entitySet = _context.Employees;
            return await entitySet.ToListAsync();

        }

        async Task<Employee> IRepository<Employee>.GetByIdAsync(Guid id)
        {
            var empls = _context.Employees
                .Include(u => u.Role);  
  
            return await empls.FirstAsync(x => x.Id == id);

        }
    }
}
