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

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount.Amount,
            Currency = order.TotalAmount.Currency,
            ShippingAddress = new AddressDto
            {
                Street = order.ShippingAddress.Street,
                City = order.ShippingAddress.City,
                State = order.ShippingAddress.State,
                Country = order.ShippingAddress.Country,
                ZipCode = order.ShippingAddress.ZipCode
            },
            OrderItems = order.OrderItems.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice.Amount,
                Currency = item.UnitPrice.Currency
            }).ToList(),
            CreatedAt = order.CreatedAt
        }).ToList();
    }
}