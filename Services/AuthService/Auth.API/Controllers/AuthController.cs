using Auth.Application.Features.Auth.Commands.LoginUser;
using Auth.Application.Features.Auth.Commands.RegisterUser;
using Auth.Application.Features.Auth.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var accessToken = await _mediator.Send(command);
        return Ok(new { token = accessToken });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        var accessToken = await _mediator.Send(command);
        return Ok(new { token = accessToken });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return BadRequest("Refresh token cookie not found.");

            var command = new RefreshTokenCommand(refreshToken);
            var newAccessToken = await _mediator.Send(command);

            return Ok(new { accessToken = newAccessToken });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok();
    }
}