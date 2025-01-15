using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        public readonly IExpandedRepository<Customer> _customerRepository;
        public readonly IExpandedRepository<PromoCode> _promoCodeRepository;
        public readonly IRepository<Employee> _employeeRepository;
        public readonly IRepository<Preference> _preferenceRepository;

        public PromoCodeService(IExpandedRepository<Customer> customerRepository, 
                                IExpandedRepository<PromoCode> promoCodeRepository, 
                                IRepository<Employee> employeeRepository, 
                                IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _employeeRepository = employeeRepository;
            _preferenceRepository = preferenceRepository;
        }


        public async Task<List<PromoCodeShortResponse>> GetPromoCodesAsync()

        {
            var promoCode = await _promoCodeRepository.GetAllAsync();

            var employeesModelList = promoCode.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName,
                }).ToList();

            return employeesModelList;


        }



        public async Task<bool> GivePromocodesToCustomers(GivePromoCodeRequest request)
        {
            //1. Найдём пользователей по предпочтениям
            var customers = await _customerRepository.GetAllAsync();
            Customer customer  = null;
            Preference prefer = null;
            foreach (Customer cstmr in customers)
            {
                foreach (CustomerPreference cstmrpref in cstmr.CustomerPreferences)
                {
                    prefer = await _preferenceRepository.GetByIdAsync(cstmrpref.PreferenceId);
                    if (prefer.Name == request.Preference)
                    {
                        customer = cstmr;

                        //1а. Найдём менеджера по имени
                        var employees = await _employeeRepository.GetAllAsync();
                        var empl = employees.FirstOrDefault(x => x.FullName == request.PartnerName);


                        //2.Создадим промокоды
                        var promoCode = new PromoCode()
                        {
                            Code = request.PromoCode,
                            ServiceInfo = request.ServiceInfo,
                            PartnerName = customer.FullName,
                            PartnerManager = empl,
                            Customer = customer,
                            Preference = prefer
                        };

                        await _promoCodeRepository.CreateNewRecordAsync(promoCode);
                    }
                }
            }
             
            if (customer != null)
                return true;
            else 
                return false;
        }

    }
}
