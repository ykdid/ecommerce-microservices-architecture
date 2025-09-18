using EventBus.Abstractions;

namespace EventBus.EventBusSubscriptionsManager;

public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }
    event EventHandler<string> OnEventRemoved;

    void AddDynamicSubscription<TH>(string eventName)
        where TH : class, IDynamicIntegrationEventHandler;

    void AddSubscription<T, TH>()
        where T : class
        where TH : class, IIntegrationEventHandler<T>;

    void RemoveSubscription<T, TH>()
        where T : class
        where TH : class, IIntegrationEventHandler<T>;

    void RemoveDynamicSubscription<TH>(string eventName)
        where TH : class, IDynamicIntegrationEventHandler;

    bool HasSubscriptionsForEvent<T>() where T : class;
    bool HasSubscriptionsForEvent(string eventName);

    Type? GetEventTypeByName(string eventName);
    void Clear();

    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : class;
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

    string GetEventKey<T>();
}