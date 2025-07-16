using Auth.Application.DTOs;

namespace Auth.Application.Abstractions.Identity;

public interface IUserService
{
    Task<(bool Succeeded, string Token, string RefreshToken, string[] Errors)> RegisterAsync(string fullName, string email, string password);
    Task<(bool Succeeded, string Token, string RefreshToken, string Error)> LoginAsync(string email, string password);
    Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> IsRefreshTokenValidAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task<RefreshTokenDto> GenerateAndSaveNewRefreshTokenAsync(string userId);
}