using System.Security.Cryptography;
using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using Auth.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Identity;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<(bool Succeeded, string Token, string RefreshToken, string[] Errors)> RegisterAsync(string fullName, string email, string password)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return (false, string.Empty, string.Empty, result.Errors.Select(e => e.Description).ToArray());

        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        return (true, token, refreshToken.Token, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string Token, string RefreshToken, string Error)> LoginAsync(string email, string password)
    {
        var user = await _userManager.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            return (false, string.Empty, string.Empty, "User not found.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return (false, string.Empty, string.Empty, "Incorrect password.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        return (true, token, refreshToken.Token, string.Empty);
    }
    public async Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

        if (user == null) return null;

        return new UserDto(user.Id, user.Email!, user.UserName!);
    }
        
    public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

        var token = user?.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

        return token is not null && token.ExpiresAt > DateTime.UtcNow && !token.IsRevoked;
    }
    
    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

        var token = user?.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
        if (token is null)
            return;

        token.IsRevoked = true;
        await _userManager.UpdateAsync(user);
    }
    
    public async Task<RefreshTokenDto> GenerateAndSaveNewRefreshTokenAsync(string userId)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            throw new Exception("User not found");

        var newRefreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        return new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.ExpiresAt);
    }
    
}