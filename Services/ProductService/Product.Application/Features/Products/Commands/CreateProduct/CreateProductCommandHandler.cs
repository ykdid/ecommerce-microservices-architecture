using MediatR;
using Product.Application.Abstractions.Repositories;
using Product.Domain.ValueObjects;

namespace Product.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Domain.Entities.Product(
            name: request.Name,
            price: new Price(request.Price),
            stock: new Stock(request.Stock)
        );

        await _productRepository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}