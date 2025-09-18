using Microsoft.EntityFrameworkCore;
using Stock.Application.Abstractions.Repositories;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class StockRepository : GenericRepository<StockItem>, IStockRepository
{
    public StockRepository(StockDbContext context) : base(context) { }

    public async Task<StockItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<StockItem>> GetLowStockItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Status != StockStatus.Discontinued)
            .AsEnumerable() // Switch to client-side evaluation for complex business logic
            .Where(s => s.IsLowStock())
            .ToListAsync();
    }

    public async Task<IEnumerable<StockItem>> GetOutOfStockItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Status == StockStatus.OutOfStock)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<StockItem>> GetByProductIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => productIds.Contains(s.ProductId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasSufficientStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stockItem = await GetByProductIdAsync(productId, cancellationToken);
        return stockItem?.HasSufficientStock(quantity) ?? false;
    }
}