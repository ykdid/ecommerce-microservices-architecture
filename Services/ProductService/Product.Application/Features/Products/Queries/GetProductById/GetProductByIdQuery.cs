using MediatR;
using Product.Application.Features.Products.Dtos;

namespace Product.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;