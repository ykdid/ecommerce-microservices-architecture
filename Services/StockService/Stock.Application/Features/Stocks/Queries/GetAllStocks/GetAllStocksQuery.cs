using MediatR;
using Stock.Application.Abstractions.Repositories;
using Stock.Application.DTOs;

namespace Stock.Application.Features.Stocks.Queries.GetAllStocks;

public record GetAllStocksQuery : IRequest<List<StockItemDto>>;

public class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, List<StockItemDto>>
{
    private readonly IStockRepository _stockRepository;

    public GetAllStocksQueryHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<List<StockItemDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
    {
        var stockItems = await _stockRepository.GetAllAsync(cancellationToken);

        return stockItems.Select(stockItem => new StockItemDto
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
        }).ToList();
    }
}