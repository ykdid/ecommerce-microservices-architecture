using Order.Domain.Entities;

namespace Order.Application.Abstractions.Repositories;

public interface IOrderRepository : IGenericRepository<Domain.Entities.Order>
{
    Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Order?> GetOrderWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
}