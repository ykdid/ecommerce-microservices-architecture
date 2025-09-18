using MediatR;
using Order.Application.DTOs;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    string UserId,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    IEnumerable<CreateOrderItemDto> OrderItems
) : IRequest<Guid>;

public sealed record CreateOrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity
);