using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Products.Commands.CreateProduct;
using Product.Application.Features.Products.Commands.DeleteProduct;
using Product.Application.Features.Products.Commands.UpdateProduct;
using Product.Application.Features.Products.Queries.GetAllProducts;
using Product.Application.Features.Products.Queries.GetProductById;

namespace Product.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = productId }, new { Id = productId });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch between route and body.");

        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}