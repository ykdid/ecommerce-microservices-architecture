using EventBus.Abstractions;
using EventBus.Configuration;
using EventBus.Events;
using EventBus.EventBusSubscriptionsManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventBus.RabbitMQ;

public class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMQSettings _settings;
    private IConnection? _connection;
    private IModel? _consumerChannel;
    private string _queueName;

    public RabbitMQEventBus(
        ILogger<RabbitMQEventBus> logger,
        IEventBusSubscriptionsManager subsManager,
        IServiceProvider serviceProvider,
        IOptions<RabbitMQSettings> settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _queueName = _settings.QueueName;
        
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        
        CreateConnection();
    }

    private void CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            
            if (!_connection.IsOpen)
            {
                throw new InvalidOperationException("RabbitMQ connection is not open");
            }

            _consumerChannel = _connection.CreateModel();
            
            _consumerChannel.ExchangeDeclare(exchange: _settings.ExchangeName, type: "direct");
            _consumerChannel.QueueDeclare(queue: _queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            _consumerChannel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");
                _consumerChannel?.Dispose();
                CreateConsumerChannel();
                StartBasicConsume();
            };

            _logger.LogInformation("RabbitMQ connection established");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not create RabbitMQ connection");
            throw;
        }
    }

    private void CreateConsumerChannel()
    {
        if (_connection?.IsOpen == true)
        {
            _consumerChannel = _connection.CreateModel();
            _consumerChannel.ExchangeDeclare(exchange: _settings.ExchangeName, type: "direct");
            _consumerChannel.QueueDeclare(queue: _queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
        }
    }

    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        if (_connection?.IsOpen != true)
        {
            CreateConnection();
        }

        var eventName = @event.GetType().Name;

        _logger.LogTrace("Publishing event to RabbitMQ: {EventId} - {EventName}", @event.Id, eventName);

        using var channel = _connection!.CreateModel();
        
        channel.ExchangeDeclare(exchange: _settings.ExchangeName, type: "direct");

        var body = JsonConvert.SerializeObject(@event);
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2; // persistent

        channel.BasicPublish(
            exchange: _settings.ExchangeName,
            routingKey: eventName,
            basicProperties: properties,
            body: bodyBytes);

        _logger.LogTrace("Published event to RabbitMQ: {EventId} - {EventName}", @event.Id, eventName);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();
        DoInternalSubscription(eventName);

        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

        _subsManager.AddSubscription<T, TH>();
        StartBasicConsume();
    }

    private void DoInternalSubscription(string eventName)
    {
        var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (_connection?.IsOpen != true)
            {
                CreateConnection();
            }

            _consumerChannel!.QueueBind(queue: _queueName,
                                      exchange: _settings.ExchangeName,
                                      routingKey: eventName);
        }
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    private void SubsManager_OnEventRemoved(object? sender, string eventName)
    {
        if (_connection?.IsOpen != true)
        {
            CreateConnection();
        }

        _consumerChannel!.QueueUnbind(queue: _queueName,
                                    exchange: _settings.ExchangeName,
                                    routingKey: eventName);

        if (_subsManager.IsEmpty)
        {
            _queueName = string.Empty;
            _consumerChannel.Close();
        }
    }

    private void StartBasicConsume()
    {
        _logger.LogTrace("Starting RabbitMQ basic consume");

        if (_consumerChannel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

            consumer.Received += Consumer_Received;

            _consumerChannel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);
        }
        else
        {
            _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
        }
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            if (message.ToLowerInvariant().Contains("throw-fake-exception"))
            {
                throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
            }

            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error processing message \"{Message}\"", message);
        }

        // Even on exception we take the message off the queue.
        // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX).
        // For more information see: https://www.rabbitmq.com/dlx.html
        _consumerChannel!.BasicAck(eventArgs.DeliveryTag, multiple: false);
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _serviceProvider.CreateScope();
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            
            foreach (var subscription in subscriptions)
            {
                if (subscription.IsDynamic)
                {
                    if (scope.ServiceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler)
                    {
                        continue;
                    }

                    using dynamic eventData = JsonConvert.DeserializeObject(message)!;
                    await Task.Yield();
                    await handler.Handle(eventData);
                }
                else
                {
                    var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;
                    
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    if (eventType == null) continue;
                    
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object[] { integrationEvent! })!;
                }
            }
        }
        else
        {
            _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
        }
    }

    public void Dispose()
    {
        if (_consumerChannel != null)
        {
            _consumerChannel.Dispose();
        }

        if (_connection != null)
        {
            _connection.Dispose();
        }

        _subsManager.Clear();
    }
}

public static class GenericTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        var typeName = string.Empty;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}