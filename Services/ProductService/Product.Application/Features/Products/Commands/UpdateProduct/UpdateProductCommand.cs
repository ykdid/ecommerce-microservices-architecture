using MediatR;

namespace Product.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price,
    int Stock
) : IRequest<Unit>;