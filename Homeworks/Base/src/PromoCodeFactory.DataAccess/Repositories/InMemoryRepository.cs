﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;



namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.Where(x => x.IsDeleted == false));
        }

        public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Data.FirstOrDefault(x => (x.Id == id) && (x.IsDeleted == false)));
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var empl = Data.FirstOrDefault(x => x.Id == id);

            if (empl != null)
            {

                empl.IsDeleted = true;
                return Task.FromResult(true);
            }
            else
                return Task.FromResult(false);

         }

        public Task<IEnumerable<T>> CreateAsync(IEnumerable<T> empl, CancellationToken cancellationToken)
        {

            Data = Data.Concat(empl);


            return Task.FromResult(Data.Where(x => x.IsDeleted == false));
        }

        public Task<T> ReplaceAsync(IEnumerable<T> empl, Guid id, CancellationToken cancellationToken)
        {
            //var empl = Data.FirstOrDefault(x => x.Id == empl.id);

            if (empl != null)
            {
                Data = Data.Where(x => x.Id != id);
                Data = Data.Concat(empl);
            }
            
            return Task.FromResult(Data.FirstOrDefault(x => (x.Id == id) && (x.IsDeleted == false)));
        }
    }
}