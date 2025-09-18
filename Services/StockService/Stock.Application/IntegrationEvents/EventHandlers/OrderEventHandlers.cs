using BuildingBlocks.EventBus.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;
using Stock.Application.Features.Stocks.Commands.ReserveStock;
using Stock.Application.Features.Stocks.Commands.ReleaseStock;

namespace Stock.Application.IntegrationEvents.EventHandlers;

public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly IEventBus _eventBus;

    public OrderCreatedIntegrationEventHandler(IMediator mediator, IEventBus eventBus)
    {
        _mediator = mediator;
        _eventBus = eventBus;
    }

    public async Task Handle(OrderCreatedIntegrationEvent @event)
    {
        foreach (var orderItem in @event.OrderItems)
        {
            try
            {
                var command = new ReserveStockCommand(orderItem.ProductId, orderItem.Quantity, @event.OrderId);
                await _mediator.Send(command);

                // Başarılı rezervasyon event'i gönder
                var stockReservedEvent = new StockReservedIntegrationEvent(
                    @event.OrderId, 
                    orderItem.ProductId, 
                    orderItem.Quantity, 
                    true);

                await _eventBus.PublishAsync(stockReservedEvent);
            }
            catch (Exception ex)
            {
                // Hata durumunda event gönder
                var stockReservedEvent = new StockReservedIntegrationEvent(
                    @event.OrderId, 
                    orderItem.ProductId, 
                    orderItem.Quantity, 
                    false, 
                    ex.Message);

                await _eventBus.PublishAsync(stockReservedEvent);
            }
        }
    }
}

public class OrderCancelledIntegrationEventHandler : IIntegrationEventHandler<OrderCancelledIntegrationEvent>
{
    private readonly IMediator _mediator;
    private readonly IEventBus _eventBus;

    public OrderCancelledIntegrationEventHandler(IMediator mediator, IEventBus eventBus)
    {
        _mediator = mediator;
        _eventBus = eventBus;
    }

    public async Task Handle(OrderCancelledIntegrationEvent @event)
    {
        foreach (var orderItem in @event.OrderItems)
        {
            try
            {
                var command = new ReleaseStockCommand(orderItem.ProductId, orderItem.Quantity, @event.OrderId);
                await _mediator.Send(command);

                // Başarılı serbest bırakma event'i gönder
                var stockReleasedEvent = new StockReleasedIntegrationEvent(
                    @event.OrderId, 
                    orderItem.ProductId, 
                    orderItem.Quantity);

                await _eventBus.PublishAsync(stockReleasedEvent);
            }
            catch (Exception)
            {
                // Hata durumunda log yazılabilir
                // Şu an için sessizce devam et
            }
        }
    }
}