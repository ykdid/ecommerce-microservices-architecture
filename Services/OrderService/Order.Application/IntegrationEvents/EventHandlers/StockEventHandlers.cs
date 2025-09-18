using BuildingBlocks.EventBus.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;
using Order.Application.Features.Orders.Commands.CreateOrder;

namespace Order.Application.IntegrationEvents.EventHandlers;

public class StockReservedIntegrationEventHandler : IIntegrationEventHandler<StockReservedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public StockReservedIntegrationEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(StockReservedIntegrationEvent @event)
    {
        if (@event.Success)
        {
            // Stok rezervasyonu başarılı - siparişi onaylayabilir veya bir sonraki adıma geçebiliriz
            // Bu durumda sipariş durumunu güncelleyebiliriz
            Console.WriteLine($"Stock reserved successfully for Order: {@event.OrderId}, Product: {@event.ProductId}");
        }
        else
        {
            // Stok rezervasyonu başarısız - siparişi iptal etmeli veya hata durumu oluşturmalıyız
            Console.WriteLine($"Stock reservation failed for Order: {@event.OrderId}, Product: {@event.ProductId}, Error: {@event.ErrorMessage}");
            
            // Burada sipariş iptal logic'i eklenebilir
            // Örneğin: CancelOrderCommand gönderilebilir
        }
    }
}

public class StockOutOfStockIntegrationEventHandler : IIntegrationEventHandler<StockOutOfStockIntegrationEvent>
{
    public async Task Handle(StockOutOfStockIntegrationEvent @event)
    {
        // Stok bitti bildirimini işle
        // Bu durumda:
        // 1. Admin'e bildirim gönderilebilir
        // 2. Müşterilere stok bitti bildirimi yapılabilir
        // 3. Log kaydı tutulabilir
        
        Console.WriteLine($"Product out of stock: {@event.ProductId} - {@event.ProductName}");
        
        // Burada notification service'e event gönderilebilir
        await Task.CompletedTask;
    }
}