using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Threading;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace PromoCodeFactory.DataAccess.Repositories
{
    public class SQLiteRepositoryRole<Role> : IRepository2
        //                                    where T : BaseEntity
        //public class SQLiteRepository
    {
        protected AppDbContext _context;

        public SQLiteRepositoryRole(AppDbContext context)
        {
            _context = context;
        }


        public Task<List<PromoCodeFactory.Core.Domain.Administration.Role>> GetAllAsync()
        {
            //throw new NotImplementedException();

            //IEnumerable<Role> userIQuer = _context.Roles;
            ////var roles = userIQuer.Where(p => p.Id <> 10).ToList();
            //var roles = userIQuer.AsEnumerable<Role>;

            ////var role = new Role();
            ////IEnumerable<Role> roleList = new List<Role>(FakeDataFactory.Roles);
            ////roleList = FakeDataFactory.Roles;

            //return Task.FromResult(roles);


            ////var entitySet = _context.Set<T>();
            ////return await entitySet;
            ////

            var res0 = _context.Roles.AsNoTracking().Where(s => s.Name != string.Empty).ToList();
            var res = (List<PromoCodeFactory.Core.Domain.Administration.Role >)_context.Roles.AsNoTracking().ToList();
            return Task.FromResult(res);


        }

        //public Task<IEnumerable<Core.Domain.Administration.Role>> IRepository2.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<PromoCodeFactory.Core.Domain.Administration.Role> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
            //var entitySet = _context.Set<Role>();
            //var res = _context.Roles.AsNoTracking().ToList();
            //return await entitySet.FindAsync(id);


            //var query = context.Set<Employee>().AsQueryable();
            //return await (AppDbContext db) => db.Employees.ToList();
        }


        //public async Task<Core.Domain.Administration.Role> IRepository2.GetByIdAsync(Guid id)
        //{
        //    //throw new NotImplementedException();
        //    var entitySet = _context.Set<Role>();
        //    return await entitySet.FindAsync(id);

        //}

        //public override async Task<Course> GetAsync(int id, CancellationToken cancellationToken)
        //{
        //    var query = Context.Set<Course>().AsQueryable();
        //    return await query.SingleOrDefaultAsync(c => c.Id == id);
        //}

    }
}
