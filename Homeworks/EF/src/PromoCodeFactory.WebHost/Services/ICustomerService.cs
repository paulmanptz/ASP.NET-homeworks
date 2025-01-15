using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Services
{
      public interface ICustomerService
    {
        System.Threading.Tasks.Task<List<CustomerShortResponse>> GetCustomersAsync();
        System.Threading.Tasks.Task<CustomerResponse> GetCustomerAsync(Guid id);
        System.Threading.Tasks.Task<bool> CreateCustomerAsync(CreateOrEditCustomerRequest request);
        System.Threading.Tasks.Task<bool> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request);
        System.Threading.Tasks.Task<bool> DeleteCustomer(Guid id);
    }
}
