using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Stocks.Commands.ReserveStock;
using Stock.Application.Features.Stocks.Commands.ReleaseStock;
using Stock.Application.Features.Stocks.Commands.UpdateStock;
using Stock.Application.Features.Stocks.Queries.GetAllStocks;
using Stock.Application.Features.Stocks.Queries.GetStockByProductId;

namespace Stock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public StocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStocks(CancellationToken cancellationToken)
    {
        var query = new GetAllStocksQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetStockByProductId(Guid productId, CancellationToken cancellationToken)
    {
        var query = new GetStockByProductIdQuery(productId);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
            return NotFound($"Stock not found for product ID: {productId}");
            
        return Ok(result);
    }

    [HttpPut("update-quantity")]
    public async Task<IActionResult> UpdateStockQuantity([FromBody] UpdateStockQuantityCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveStock([FromBody] ReserveStockCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("release")]
    public async Task<IActionResult> ReleaseStock([FromBody] ReleaseStockCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}