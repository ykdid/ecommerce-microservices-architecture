using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;

namespace Auth.Application.Features.Auth.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        ITokenService tokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            throw new Exception("Refresh token not found.");

        var isValid = await _userService.IsRefreshTokenValidAsync(refreshToken);
        if (!isValid)
            throw new Exception("Refresh token invalid or expired.");

        var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);
        if (user is null)
            throw new Exception("User not found.");

        await _userService.RevokeRefreshTokenAsync(refreshToken);

        var roles = new List<string> { "User" };
        var accessToken = _tokenService.GenerateToken(user.Id, user.Email, roles);

        var newRefreshToken = await _userService.GenerateAndSaveNewRefreshTokenAsync(user.Id);

        _httpContextAccessor.HttpContext!.Response.Cookies.Append(
            "refreshToken",
            newRefreshToken.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.ExpiresAt
            });

        return accessToken;
    }
}