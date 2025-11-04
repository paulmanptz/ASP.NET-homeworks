using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways
{
    public interface IGivingPromoCodeToCustomerGateway
    {
        //Task GivePromoCodeToCustomer(PromoCodeFromPartner promoCode);
        void GivePromoCodeToCustomer(PromoCode promoCode);
    }
}