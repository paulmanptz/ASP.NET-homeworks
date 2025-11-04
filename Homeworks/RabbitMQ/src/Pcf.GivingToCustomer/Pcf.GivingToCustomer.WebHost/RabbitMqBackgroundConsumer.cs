using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.PromocodeService;
using Pcf.ReceivingFromPartner.Integration.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost
{
    public class RabbitMqBackgroundConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private RabbitMQ.Client.IModel? _channel;
        //private readonly IPromoCodeService _promoCodeService;
        //private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IServiceProvider _serviceProvider;

        //public RabbitMqBackgroundConsumer(IConfiguration configuration, IPromoCodeService promoCodeService, IRepository<PromoCode> promoCodesRepository, IServiceProvider serviceProvider)
        public RabbitMqBackgroundConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            //_promoCodeService = promoCodeService;
            //_promoCodesRepository = promoCodesRepository;
            _serviceProvider = serviceProvider;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    ConnectToRabbitMq(stoppingToken);

        //    // Запускаем потребителя
        //    //RegisterConsumer("Promocode", "Code1", "Promocode.1");
        //    RegisterConsumer("Promocodes", "QueueForCustomers", "PcfRkCust");

        //    // Держим сервис живым, пока не будет запрос на остановку
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectToRabbitMq(stoppingToken);

            RegisterConsumer("Promocodes", "QueueForCustomers", "PcfRkCust");

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

        //private void ConnectToRabbitMq(CancellationToken stoppingToken)
        //{
        //    RmqSettings rmqSettings = _configuration.GetSection("RmqSettings").Get<RmqSettings>();

        //    ConnectionFactory factory = new ConnectionFactory()
        //    {
        //        HostName = rmqSettings.Host,
        //        VirtualHost = rmqSettings.VHost,
        //        UserName = rmqSettings.Login,
        //        Password = rmqSettings.Password
        //    };

        //    _connection = factory.CreateConnection();
        //    _channel = _connection.CreateModel();

        //    Console.WriteLine("Подключено к RabbitMQ");
        //}

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

        //private void RegisterConsumer(string exchangeName, string queueName, string routingKey)
        //{
        //    if (_channel == null) throw new InvalidOperationException("Канал не инициализирован");

        //    _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        //    // Объявляем exchange
        //    _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

        //    // Объявляем очередь
        //    _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        //    // Привязываем очередь к exchange
        //    _channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);

        //    // Создаём потребителя
        //    EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        //    consumer.Received += async (sender, e) =>
        //    {
        //        try
        //        {
        //            byte[] body = e.Body.ToArray();
        //            string json = Encoding.UTF8.GetString(body);

        //            GivePromoCodeToCustomerDto message = JsonSerializer.Deserialize<GivePromoCodeToCustomerDto>(json);

        //            GivePromoCodeRequest request = new GivePromoCodeRequest()
        //            {
        //                BeginDate = message.BeginDate,
        //                EndDate = message.EndDate,
        //                PreferenceId = message.PreferenceId,
        //                PromoCode = message.PromoCode,
        //                PromoCodeId = message.PromoCodeId,
        //                PartnerId = message.PartnerId,
        //                ServiceInfo = message.ServiceInfo
        //            };
        //            PromoCode promoCode = await _promoCodeService.CreatePromoCodeForPreferenceAsync(request);

        //            await _promoCodesRepository.AddAsync(promoCode);


        //            Console.WriteLine($"{DateTime.Now} Received: {message?.PromoCode}");

        //            // Подтверждаем приём
        //            _channel.BasicAck(e.DeliveryTag, multiple: false);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
        //            _channel.BasicNack(e.DeliveryTag, multiple: false, requeue: true); // Вернуть в очередь
        //        }
        //    };

        //    // Запускаем потребление
        //    _channel.BasicConsume(queueName, autoAck: false, consumer: consumer);

        //    Console.WriteLine($"Потребитель запущен: queue='{queueName}', key='{routingKey}'");
        //}

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
                // 🔥 Создаём scope для каждого сообщения
                using IServiceScope scope = _serviceProvider.CreateScope();

                try
                {
                    byte[] body = e.Body.ToArray();
                    string json = Encoding.UTF8.GetString(body);

                    GivePromoCodeToCustomerDto message = JsonSerializer.Deserialize<GivePromoCodeToCustomerDto>(json);

                    GivePromoCodeRequest request = new GivePromoCodeRequest
                    {
                        BeginDate = message.BeginDate,
                        EndDate = message.EndDate,
                        PreferenceId = message.PreferenceId,
                        PromoCode = message.PromoCode,
                        PromoCodeId = message.PromoCodeId,
                        PartnerId = message.PartnerId,
                        ServiceInfo = message.ServiceInfo
                    };

                    // Получаем сервисы из нового scope
                    IPromoCodeService promoCodeService = scope.ServiceProvider.GetRequiredService<IPromoCodeService>();
                    IRepository<PromoCode> promoCodeRepository = scope.ServiceProvider.GetRequiredService<IRepository<PromoCode>>();

                    PromoCode promoCode = await promoCodeService.CreatePromoCodeForPreferenceAsync(request);
                    await promoCodeRepository.AddAsync(promoCode);

                    Console.WriteLine($"{DateTime.Now} Received: {message.PromoCode}");

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
