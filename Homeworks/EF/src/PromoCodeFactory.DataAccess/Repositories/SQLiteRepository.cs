using PromoCodeFactory.Core.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Threading;
using PromoCodeFactory.Core.Domain.Administration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace PromoCodeFactory.DataAccess.Repositories
{
    public class SQLiteRepository<T> : IRepository<T>
                                 where T : BaseEntity
        //public class SQLiteRepository
    {
        protected AppDbContext _context;

        public SQLiteRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //throw new NotImplementedException();


            var entitySet = _context.Set<T>();
            return await entitySet.ToListAsync();


        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            var entitySet = _context.Set<T>();
            return await entitySet.FindAsync(id);


            //var query = context.Set<Employee>().AsQueryable();
            //return await (AppDbContext db) => db.Employees.ToList();
        }
        //public override async Task<Course> GetAsync(int id, CancellationToken cancellationToken)
        //{
        //    var query = Context.Set<Course>().AsQueryable();
        //    return await query.SingleOrDefaultAsync(c => c.Id == id);
        //}

        public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var entitySet = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
                entitySet = entitySet.Include(include);

            return await entitySet.FirstAsync(x => x.Id == id);
        }

        public async Task CreateNewRecordAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecordAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecordAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}