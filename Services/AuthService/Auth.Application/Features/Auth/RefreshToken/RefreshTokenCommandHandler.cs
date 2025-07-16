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
        if (string.IsNullOrWhiteSpace(request.RefreshTokenFromCookie))
            throw new Exception("Refresh token not found in cookie");

        var isValid = await _userService.IsRefreshTokenValidAsync(request.RefreshTokenFromCookie);

        if (!isValid)
            throw new Exception("Invalid refresh token");

        var user = await _userService.GetUserByRefreshTokenAsync(request.RefreshTokenFromCookie);

        if (user is null)
            throw new Exception("User not found");

        await _userService.RevokeRefreshTokenAsync(request.RefreshTokenFromCookie);

        var newRefreshTokenDto = await _userService.GenerateAndSaveNewRefreshTokenAsync(user.Id);

        var accessToken = await _userService.GenerateAccessTokenAsync(user);

        _userService.AppendRefreshTokenCookie(newRefreshTokenDto.Token, newRefreshTokenDto.ExpiresAt);

        return accessToken;
    }
}