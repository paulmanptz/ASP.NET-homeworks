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

            // ✅ Для v6.2.2: используем синхронное создание подключения
            _connection = factory.CreateConnection(new[] { new AmqpTcpEndpoint(factory.HostName) });
        }

        public void SendMessage(object message)
        {
            //using var channel = await _connection.CreateChannelAsync();
            using IModel channel = _connection.CreateModel();

            //// Объявление очереди
            //channel.QueueDeclare(
            //    queue: "PcfGivingToCustomer",
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false,
            //    arguments: null);

            string json = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(json);

            //// Создание свойств (в v6.2.2 CreateBasicProperties() есть у канала)
            //IBasicProperties properties = channel.CreateBasicProperties();
            //properties.ContentType = "application/json";

            // Отправка — используем стандартный BasicPublishAsync без generic
            channel.BasicPublish(
                exchange: "Promocodes",
                routingKey: "PcfRkCust",
                mandatory: false,
                null,//basicProperties: properties,
                body: body // ✅ Передаём byte[], не ReadOnlyMemory<byte>
            );

            Console.WriteLine($"Pcf sent event: {message}");
        }

    }
}
