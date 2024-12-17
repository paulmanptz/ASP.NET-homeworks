using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);

        Task CreateNewRecordAsync(T entity);
        Task UpdateRecordAsync(T entity);
        Task DeleteRecordAsync(T entity);

    }
}