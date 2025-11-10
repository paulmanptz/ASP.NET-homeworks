using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.WebHost.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService service)
        {
            _customerService = service;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            return Ok(await _customerService.GetCustomersAsync());
        }

        /// <summary>
        /// получить информацию по конкретному клиенту 
        /// </summary>
        /// <param name="id">ИД клиента</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var model = await _customerService.GetCustomerAsync(id);
            if (model == null)
                return NotFound();
            else
                return Ok(model);
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var result = await _customerService.CreateCustomerAsync(request);
            if (result)
                return Ok("Created");
            else
                return BadRequest("Customer preference not found");
        }

        /// <summary>
        /// Изменить данные клиента
        /// </summary>
        /// <param name="id">Ид клиент</param>
        /// <param name="request">новый экземпляр данных клиента</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var result = await _customerService.EditCustomersAsync(id, request);
            if (result)
                return Ok("Updated");
            else
                return BadRequest("Customer or customer preference not found");

        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">Ид клиента</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteCustomer(id);
            if (result)
                return Ok("Deleted");
            else
                return NotFound();

        }
    }
}