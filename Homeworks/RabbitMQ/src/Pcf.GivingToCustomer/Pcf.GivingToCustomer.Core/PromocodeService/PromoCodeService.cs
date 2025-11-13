using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Mappers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.PromocodeService
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PromoCodeService(
            IRepository<Preference> preferencesRepository,
            IRepository<Customer> customersRepository)
        {
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }

        public async Task<PromoCode> CreatePromoCodeForPreferenceAsync(GivePromoCodeRequest request, CancellationToken cancellationToken = default)
        {
            // Получаем предпочтение
            Preference preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

            if (preference == null)
                throw new InvalidOperationException($"Preference with ID {request.PreferenceId} not found.");

            // Получаем клиентов с этим предпочтением
            System.Collections.Generic.IEnumerable<Customer> customers = await _customersRepository.GetWhere(c =>
                c.Preferences.Any(p => p.Preference.Id == preference.Id));

            // Создаём промокод
            PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, customers);

            return promoCode;
        }
    }
}
