using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IExpandedRepository<T> : IRepository<T>
          where T : BaseEntity
    {
        public Task CreateNewRecordAsync(T entity);
        public Task UpdateRecordAsync(T entity);
        public Task DeleteRecordAsync(T entity);    
    }

}
