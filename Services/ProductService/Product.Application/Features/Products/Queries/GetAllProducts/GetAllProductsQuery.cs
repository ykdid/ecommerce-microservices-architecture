using MediatR;
using Product.Application.Features.Products.Dtos;

namespace Product.Application.Features.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;