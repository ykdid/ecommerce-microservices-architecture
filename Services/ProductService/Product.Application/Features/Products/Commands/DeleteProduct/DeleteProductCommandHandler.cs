using MediatR;
using Product.Application.Abstractions.Repositories;

namespace Product.Application.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
                      ?? throw new Exception("Product not found.");

        if (product.IsDeleted)
        {
            throw new Exception("Product already deleted");
        }

        product.Delete();
        await _productRepository.UpdateAsync(product, cancellationToken);
        
        return Unit.Value;
    }
}