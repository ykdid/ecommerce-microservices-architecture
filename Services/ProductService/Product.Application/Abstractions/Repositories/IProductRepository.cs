namespace Product.Application.Abstractions.Repositories;

public interface IProductRepository : IGenericRepository<Domain.Entities.Product>
{
    Task<Domain.Entities.Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}