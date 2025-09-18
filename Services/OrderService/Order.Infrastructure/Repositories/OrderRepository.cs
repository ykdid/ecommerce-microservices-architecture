using Microsoft.EntityFrameworkCore;
using Order.Application.Abstractions.Repositories;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Domain.Entities.Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context) { }

    public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Order?> GetOrderWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
}