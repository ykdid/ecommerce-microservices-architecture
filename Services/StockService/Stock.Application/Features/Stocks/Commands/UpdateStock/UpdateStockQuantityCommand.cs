using FluentValidation;
using MediatR;
using Stock.Application.Abstractions.Repositories;
using Stock.Application.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Features.Stocks.Commands.UpdateStock;

public record UpdateStockQuantityCommand(Guid ProductId, int NewQuantity) : IRequest<StockItemDto>;

public class UpdateStockQuantityCommandValidator : AbstractValidator<UpdateStockQuantityCommand>
{
    public UpdateStockQuantityCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.NewQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative");
    }
}

public class UpdateStockQuantityCommandHandler : IRequestHandler<UpdateStockQuantityCommand, StockItemDto>
{
    private readonly IStockRepository _stockRepository;

    public UpdateStockQuantityCommandHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockItemDto> Handle(UpdateStockQuantityCommand request, CancellationToken cancellationToken)
    {
        var stockItem = await _stockRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        
        if (stockItem == null)
            throw new InvalidOperationException($"Stock item not found for product ID: {request.ProductId}");

        stockItem.UpdateQuantity(request.NewQuantity);
        
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