using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Integration.Dto;
using Pcf.ReceivingFromPartner.Integration.RabbitMQ;
using System.Net.Http;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGateway
        : IGivingPromoCodeToCustomerGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IRabbitMqProducer _producer;

        public GivingPromoCodeToCustomerGateway(HttpClient httpClient, IRabbitMqProducer producer)
        {
            _httpClient = httpClient;
            _producer = producer;
        }

        //public async Task GivePromoCodeToCustomer(PromoCode promoCode)
        //{
        //    var dto = new GivePromoCodeToCustomerDto()
        //    {
        //        PartnerId = promoCode.Partner.Id,
        //        BeginDate = promoCode.BeginDate.ToShortDateString(),
        //        EndDate = promoCode.EndDate.ToShortDateString(),
        //        PreferenceId = promoCode.PreferenceId,
        //        PromoCode = promoCode.Code,
        //        ServiceInfo = promoCode.ServiceInfo,
        //        PartnerManagerId = promoCode.PartnerManagerId
        //    };

        //    var response = await _httpClient.PostAsJsonAsync("api/v1/promocodes", dto);

        //    response.EnsureSuccessStatusCode();
        //}
        public void GivePromoCodeToCustomer(PromoCode promoCode)
        {
            GivePromoCodeToCustomerDto dto = new GivePromoCodeToCustomerDto()
            {
                PartnerId = promoCode.Partner.Id,
                BeginDate = promoCode.BeginDate.ToShortDateString(),
                EndDate = promoCode.EndDate.ToShortDateString(),
                PreferenceId = promoCode.PreferenceId,
                PromoCode = promoCode.Code,
                ServiceInfo = promoCode.ServiceInfo,
                PartnerManagerId = promoCode.PartnerManagerId
            };

            _producer.SendMessage(dto);
            //HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/v1/promocodes", dto);

            //response.EnsureSuccessStatusCode();
        }

    }
}