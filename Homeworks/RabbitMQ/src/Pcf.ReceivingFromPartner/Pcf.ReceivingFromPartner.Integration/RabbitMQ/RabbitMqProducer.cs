using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Pcf.ReceivingFromPartner.Integration.RabbitMQ
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly IConnection _connection;

        public RabbitMqProducer(IConfiguration configuration)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                VirtualHost = configuration["RabbitMQ:VHost"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            // Для v6.2.2: используем синхронное создание подключения
            _connection = factory.CreateConnection(new[] { new AmqpTcpEndpoint(factory.HostName) });
        }

        public void SendMessage(object message, string routingKey)
        {
            using IModel channel = _connection.CreateModel();

            string json = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(json);

            // Отправка — используем стандартный BasicPublishAsync без generic
            channel.BasicPublish(
                exchange: "Promocodes",
                routingKey: routingKey,
                mandatory: false,
                null,//basicProperties: properties,
                body: body // Передаём byte[], не ReadOnlyMemory<byte>
            );

            Console.WriteLine($"Pcf sent event: {message}");
        }

    }
}
