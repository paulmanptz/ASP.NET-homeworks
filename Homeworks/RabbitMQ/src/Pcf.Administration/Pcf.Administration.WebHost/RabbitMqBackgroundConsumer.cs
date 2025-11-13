using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pcf.Administration.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost
{
    public class RabbitMqBackgroundConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private RabbitMQ.Client.IModel? _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqBackgroundConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectToRabbitMq(stoppingToken);

            RegisterConsumer("Promocodes", "QueueForAdministration", "PcfRkAdm");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            finally
            {
                _channel?.Close();
                _connection?.Close();
            }
        }


        private void ConnectToRabbitMq(CancellationToken stoppingToken)
        {
            RmqSettings rmqSettings = _configuration.GetSection("RmqSettings").Get<RmqSettings>();

            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = rmqSettings.Host,
                VirtualHost = rmqSettings.VHost,
                UserName = rmqSettings.Login,
                Password = rmqSettings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            Console.WriteLine("Подключено к RabbitMQ");
        }


        private void RegisterConsumer(string exchangeName, string queueName, string routingKey)
        {
            if (_channel == null) throw new InvalidOperationException("Канал не инициализирован");

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, e) =>
            {
                // Создаём scope для каждого сообщения
                using IServiceScope scope = _serviceProvider.CreateScope();

                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);

                    var message = JsonSerializer.Deserialize<Guid>(json);


                    // Получаем сервисы из нового scope
                    IEmployeeService employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                    await employeeService.IncrementAppliedPromocodesAsync(message);


                    Console.WriteLine($"{DateTime.Now} Received: {message}");

                    _channel.BasicAck(e.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
                    _channel.BasicNack(e.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"Потребитель запущен: queue='{queueName}', key='{routingKey}'");
        }
        public override void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            base.Dispose();
        }
    }

    // Вспомогательные классы
    public class RmqSettings
    {
        public string Host { get; set; } = null!;
        public string VHost { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
