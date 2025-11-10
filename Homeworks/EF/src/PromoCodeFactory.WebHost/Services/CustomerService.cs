using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.WebHost.Services;



namespace PromoCodeFactory.WebHost.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly IExpandedRepository<Customer> _customerRepository
;       public readonly IExpandedRepository<PromoCode> _promoCodeRepository;
        public readonly IRepository<Preference> _preferenceRepository;

        public CustomerService(IExpandedRepository<Customer> customerRepository, IExpandedRepository<PromoCode> promoCodeRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
        }

        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var model = customers.Select(x => new CustomerShortResponse()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
            }).ToList();
            return model;

        }

        public async Task<CustomerResponse> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return null;

            var model = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
            };

            var promoCodeList = new List<PromoCodeShortResponse>();
            foreach (PromoCode promoCode in customer.Promocodes)
            {
                var promoCodeShort = new PromoCodeShortResponse()
                {
                    Id = promoCode.Id,
                    Code = promoCode.Code,
                    ServiceInfo = promoCode.ServiceInfo,
                    BeginDate = promoCode.BeginDate.ToString(),
                    EndDate = promoCode.EndDate.ToString(),
                    PartnerName = promoCode.PartnerName,
                };
                promoCodeList.Add(promoCodeShort);
            }

            model.PromoCodes = promoCodeList;

            return model;

        }

        public async Task<bool> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var newCustomerId = Guid.NewGuid();

            var customerPreferences = new List<CustomerPreference>();

            bool errorFlag = false;
            foreach (Guid prefId in request.PreferenceIds)
            {
                var pref = _preferenceRepository.GetByIdAsync(prefId);
 
                if (pref.Result == null)
                    errorFlag = true;
                else
                {
                    customerPreferences.Add(
                    new CustomerPreference()
                    {
                        CustomerId = newCustomerId,
                        PreferenceId = prefId
                    }
                    );
                }

            }

            if (!errorFlag)
            {
                var newCustomer = new Customer()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Id = newCustomerId,
                    CustomerPreferences = customerPreferences
                };

                await _customerRepository.CreateNewRecordAsync(newCustomer);

                return true;
            }
            else
                return false;

        }

        public async Task<bool> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return false;

            var customerPreferences = new List<CustomerPreference>();

            bool errorFlag = false;
            foreach (Guid prefId in request.PreferenceIds)
            {
                var pref = _preferenceRepository.GetByIdAsync(prefId);

                if (pref.Result == null)
                    errorFlag = true;
                else
                {
                    customerPreferences.Add(
                    new CustomerPreference()
                    {
                        CustomerId = customer.Id,
                        PreferenceId = prefId
                    }
                    );
                }

            }

            if (!errorFlag)
            {
                var changeCustomer = new Customer()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Id = id,
                    CustomerPreferences = customerPreferences
                };

                await _customerRepository.UpdateRecordAsync(changeCustomer);
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return false;

            _promoCodeRepository.DeleteRecordsAsync(customer.Promocodes);

            await _customerRepository.DeleteRecordAsync(customer);
            return true;
        }

    }
}
