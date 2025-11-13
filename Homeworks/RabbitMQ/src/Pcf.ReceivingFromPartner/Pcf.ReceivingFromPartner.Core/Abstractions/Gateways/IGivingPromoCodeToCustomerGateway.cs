using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways
{
    public interface IGivingPromoCodeToCustomerGateway
    {
        void GivePromoCodeToCustomer(PromoCode promoCode);
    }
}