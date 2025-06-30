using MediatR;

namespace Product.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int Stock
) : IRequest<Guid>;