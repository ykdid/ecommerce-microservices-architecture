namespace EventBus.Configuration;

public class RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";
    
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "ecommerce_event_bus";
    public string QueueName { get; set; } = string.Empty;
    public int RetryCount { get; set; } = 3;
}