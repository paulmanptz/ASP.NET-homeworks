namespace Pcf.ReceivingFromPartner.Integration.RabbitMQ
{
    public interface IRabbitMqProducer
    {
        void SendMessage(object message);
    }
}
