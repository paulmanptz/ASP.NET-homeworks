using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Services
{
    public interface IPromoCodeService
    {
        public Task<bool> GivePromocodesToCustomers(GivePromoCodeRequest request);
        public Task<List<PromoCodeShortResponse>> GetPromoCodesAsync();
    }
}