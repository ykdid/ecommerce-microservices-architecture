using BuildingBlocks.EventBus.IntegrationEvents;
using EventBus.Abstractions;
using MediatR;
using Order.Application.Abstractions.Repositories;
using Order.Domain.ValueObjects;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var shippingAddress = new Address(
            request.ShippingAddress.Street,
            request.ShippingAddress.City,
            request.ShippingAddress.State,
            request.ShippingAddress.Country,
            request.ShippingAddress.ZipCode);

        var billingAddress = new Address(
            request.BillingAddress.Street,
            request.BillingAddress.City,
            request.BillingAddress.State,
            request.BillingAddress.Country,
            request.BillingAddress.ZipCode);

        var order = new Domain.Entities.Order(
            request.UserId,
            shippingAddress,
            billingAddress);

        foreach (var item in request.OrderItems)
        {
            var unitPrice = new Money(item.UnitPrice, item.Currency);
            order.AddOrderItem(item.ProductId, item.ProductName, unitPrice, item.Quantity);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        var orderItems = request.OrderItems.Select(item => new OrderItemData
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Currency = item.Currency
        }).ToList();

        var integrationEvent = new OrderCreatedIntegrationEvent(
            order.Id,
            order.UserId,
            orderItems,
            order.TotalAmount.Amount,
            order.TotalAmount.Currency
        );

        await _eventBus.PublishAsync(integrationEvent);

        return order.Id;
    }
}