using MediatR;
using Order.Application.Abstractions.Repositories;
using Order.Application.DTOs;

namespace Order.Application.Features.Orders.Queries.GetOrdersByUserId;

public record GetOrdersByUserIdQuery(string UserId) : IRequest<List<OrderDto>>;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId, cancellationToken);

        return orders.Select(order => new OrderDto(
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
        )).ToList();
    }
}