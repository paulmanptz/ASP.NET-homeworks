﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);
        //Task GetByIdAsync<T1>(Guid prefId);
    }

    //public interface IRepository
    //{
    //    Task<IEnumerable<> GetAllAsync();

    //    Task<> GetByIdAsync(Guid id);
    //}
}

