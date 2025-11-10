using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferencesController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить данные сотрудника по id
        /// </summary>
        /// <returns></returns>
        public async Task<List<PreferenceResponse>> GetAllPreferences()
        {
            var pref = await _preferenceRepository.GetAllAsync();
            var prefModel = pref.Select(x =>
                new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).OrderBy(x => x.Name).ToList();
            return prefModel;
        }

    }
}
