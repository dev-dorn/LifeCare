using System.Text;
using System.Text.Json;
using LifeCare.Personnel.Application.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace LifeCare.Personnel.Infrastructure.Messaging;

public class RabbitMqEventBus : IEventBus, IAsyncDisposable
{
    private const string ExchangeName = "lifecare.events";
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqEventBus> _logger;

    public RabbitMqEventBus(
        string hostname,
        string username,
        string password,
        ILogger<RabbitMqEventBus> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password,
            ClientProvidedName = "lifecare:personnel:eventbus"
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
                ExchangeName,
                ExchangeType.Topic,
                true,
                false)
            .GetAwaiter()
            .GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

    public async Task PublishAsync<T>(T @event) where T : class
    {
        try
        {
            var eventName = @event.GetType().Name;
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await _channel.BasicPublishAsync(
                ExchangeName,
                $"personnel.{eventName}",
                false,
                properties,
                body);

            _logger.LogInformation("Published event {EventName}", eventName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event");
            throw;
        }
    }
}