using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository2
    //    where T : BaseEntity
    {
        Task<List<Role>> GetAllAsync();

        Task<Role> GetByIdAsync(Guid id);
    }
}
