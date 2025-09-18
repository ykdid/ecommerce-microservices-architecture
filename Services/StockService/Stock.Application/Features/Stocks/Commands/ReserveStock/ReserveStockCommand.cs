using FluentValidation;
using MediatR;
using Stock.Application.Abstractions.Repositories;
using Stock.Application.DTOs;

namespace Stock.Application.Features.Stocks.Commands.ReserveStock;

public record ReserveStockCommand(Guid ProductId, int Quantity, Guid OrderId) : IRequest<StockItemDto>;

public class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
{
    public ReserveStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required");
    }
}

public class ReserveStockCommandHandler : IRequestHandler<ReserveStockCommand, StockItemDto>
{
    private readonly IStockRepository _stockRepository;

    public ReserveStockCommandHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockItemDto> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var stockItem = await _stockRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        
        if (stockItem == null)
            throw new InvalidOperationException($"Stock item not found for product ID: {request.ProductId}");

        stockItem.ReserveStock(request.Quantity, request.OrderId);
        
        await _stockRepository.UpdateAsync(stockItem, cancellationToken);
        await _stockRepository.SaveChangesAsync(cancellationToken);

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