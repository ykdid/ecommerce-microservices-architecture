using MediatR;
using Product.Application.Abstractions.Repositories;
using Product.Application.Features.Products.Dtos;

namespace Product.Application.Features.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new Exception("Product not found.");

        return new ProductDto(product.Id, product.Name, product.Price.Amount, product.Stock.Quantity);
    }
}