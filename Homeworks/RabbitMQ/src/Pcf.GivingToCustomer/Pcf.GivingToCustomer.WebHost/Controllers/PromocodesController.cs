using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.PromocodeService;
using Pcf.GivingToCustomer.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GivePromoCodeRequest = Pcf.GivingToCustomer.Core.PromocodeService.GivePromoCodeRequest;


namespace Pcf.GivingToCustomer.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IPromoCodeService _promoCodeService;

        public PromocodesController(IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository, IPromoCodeService promoCodeService)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
            _promoCodeService = promoCodeService;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            IEnumerable<PromoCode> promocodes = await _promoCodesRepository.GetAllAsync();

            List<PromoCodeShortResponse> response = promocodes.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerId = x.PartnerId,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            ////Получаем предпочтение по имени
            //var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

            //if (preference == null)
            //{
            //    return BadRequest();
            //}

            ////  Получаем клиентов с этим предпочтением:
            //var customers = await _customersRepository
            //    .GetWhere(d => d.Preferences.Any(x =>
            //        x.Preference.Id == preference.Id));

            //PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, customers);

            //await _promoCodesRepository.AddAsync(promoCode);

            //return CreatedAtAction(nameof(GetPromocodesAsync), new { }, null);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                PromoCode promoCode = await _promoCodeService.CreatePromoCodeForPreferenceAsync(request);

                await _promoCodesRepository.AddAsync(promoCode);

                return CreatedAtAction(nameof(GetPromocodesAsync), new { }, null);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}