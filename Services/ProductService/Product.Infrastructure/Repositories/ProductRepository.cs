using Microsoft.EntityFrameworkCore;
using Product.Application.Abstractions.Repositories;
using Product.Infrastructure.Persistence;

namespace Product.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Domain.Entities.Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<Domain.Entities.Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }
}