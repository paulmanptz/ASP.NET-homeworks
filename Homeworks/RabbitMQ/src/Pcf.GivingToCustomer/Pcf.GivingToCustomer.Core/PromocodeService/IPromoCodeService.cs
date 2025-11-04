using Pcf.GivingToCustomer.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.PromocodeService
{
    public interface IPromoCodeService
    {
        Task<PromoCode> CreatePromoCodeForPreferenceAsync(GivePromoCodeRequest request, CancellationToken cancellationToken = default);
    }
}
