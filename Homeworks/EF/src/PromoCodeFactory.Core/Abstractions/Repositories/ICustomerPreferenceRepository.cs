﻿using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    internal interface ICustomerPreferenceRepository
    {
        public Task CreateNewRecordAsync(CustomerPreference entity);
        public Task DeleteRecordAsync(CustomerPreference entity);

    }
}

