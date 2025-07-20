using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;

namespace Auth.Application.Features.Auth.RefreshToken;
public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly IUserService _userService;

    public RefreshTokenCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenFromCookie = Uri.UnescapeDataString(request.RefreshTokenFromCookie);

        if (!await _userService.IsRefreshTokenValidAsync(tokenFromCookie))
            throw new Exception("Invalid refresh token");

        var (newAccessToken, _) = await _userService.RotateRefreshTokenAsync(tokenFromCookie);
    
        return newAccessToken;
    }
}