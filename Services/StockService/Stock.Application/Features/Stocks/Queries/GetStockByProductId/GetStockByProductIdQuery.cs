using MediatR;
using Stock.Application.Abstractions.Repositories;
using Stock.Application.DTOs;

namespace Stock.Application.Features.Stocks.Queries.GetStockByProductId;

public record GetStockByProductIdQuery(Guid ProductId) : IRequest<StockItemDto?>;

public class GetStockByProductIdQueryHandler : IRequestHandler<GetStockByProductIdQuery, StockItemDto?>
{
    private readonly IStockRepository _stockRepository;

    public GetStockByProductIdQueryHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockItemDto?> Handle(GetStockByProductIdQuery request, CancellationToken cancellationToken)
    {
        var stockItem = await _stockRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        if (stockItem == null)
            return null;

        return new StockItemDto
        {
            Id = stockItem.Id,
            ProductId = stockItem.ProductId,
            ProductName = stockItem.ProductName,
            SKU = stockItem.SKU,
            Quantity = stockItem.Quantity,
            ReservedQuantity = stockItem.ReservedQuantity,
            AvailableQuantity = stockItem.AvailableQuantity,
            MinimumStock = stockItem.MinimumStock,
            Status = stockItem.Status.ToString(),
            IsLowStock = stockItem.IsLowStock(),
            LastUpdated = stockItem.LastUpdated,
            CreatedAt = stockItem.CreatedAt
        };
    }
}