using MediatR;
using Order.Application.Abstractions.Repositories;
using Order.Application.DTOs;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithItemsAsync(request.Id, cancellationToken);

        if (order == null)
            return null;

        return new OrderDto(
            order.Id,
            order.UserId,
            order.Status.ToString(),
            new AddressDto(
                order.ShippingAddress.Street,
                order.ShippingAddress.City,
                order.ShippingAddress.State,
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode
            ),
            new AddressDto(
                order.BillingAddress.Street,
                order.BillingAddress.City,
                order.BillingAddress.State,
                order.BillingAddress.Country,
                order.BillingAddress.ZipCode
            ),
            order.TotalAmount.Amount,
            order.TotalAmount.Currency,
            order.CreatedAt,
            order.UpdatedAt,
            order.ShippedAt,
            order.DeliveredAt,
            order.CancellationReason,
            order.OrderItems.Select(item => new OrderItemDto(
                item.Id,
                item.ProductId,
                item.ProductName,
                item.UnitPrice.Amount,
                item.UnitPrice.Currency,
                item.Quantity,
                item.UnitPrice.Amount * item.Quantity
            )).ToList()
        );
    }
}