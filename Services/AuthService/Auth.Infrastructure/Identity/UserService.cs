using System.Security.Cryptography;
using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using Auth.Application.DTOs;
using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Identity;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthDbContext _dbContext;

    public UserService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        AuthDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<(bool Succeeded, string Token, string[] Errors)> RegisterAsync(string fullName, string email, string password)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            UserName = email,
            Email = email
        };

        // 1. Kullanıcıyı oluştur
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return (false, string.Empty, result.Errors.Select(e => e.Description).ToArray());

        // 2. Role ekle
        await _userManager.AddToRoleAsync(user, "User");

        // 3. Token oluştur
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        // 4. Refresh token oluştur ve KAYDET (DbContext ile)
        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken); // <-- Direkt DbContext ile ekleme
        await _dbContext.SaveChangesAsync(); // <-- Değişiklikleri kaydet

        // 5. Cookie'yi ayarla
        AppendRefreshTokenCookie(refreshToken.Token, refreshToken.ExpiresAt);

        return (true, token, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string Token, string Error)> LoginAsync(string email, string password)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            return (false, string.Empty, "User not found.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return (false, string.Empty, "Incorrect password.");
        
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);
        
        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
        
        AppendRefreshTokenCookie(refreshToken.Token, refreshToken.ExpiresAt);

        return (true, token, string.Empty);
    }
    public async Task<UserDto?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

        if (user == null) return null;
        
        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto(user.Id, user.Email!, user.UserName!, roles);
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
    
    public void AppendRefreshTokenCookie(string refreshToken, DateTime expiresAt)
    {
        var encodedToken = Uri.EscapeDataString(refreshToken);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = _httpContextAccessor.HttpContext?.Request.IsHttps ?? false,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt
        };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", encodedToken, cookieOptions);
    }
    
    public async Task<string> GenerateAccessTokenAsync(UserDto userDto)
    {
        var user = await _userManager.FindByIdAsync(userDto.Id);
        if (user is null)
            throw new Exception("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        return _tokenService.GenerateToken(user.Id, user.Email!, roles);
    }
    
    public async Task<(string AccessToken, string RefreshToken)> RotateRefreshTokenAsync(string oldRefreshToken)
    {
        await RevokeRefreshTokenAsync(oldRefreshToken);
        
        var user = await GetUserByRefreshTokenAsync(oldRefreshToken);
        if (user == null) throw new Exception("User not found");
        
        var oldTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == user.Id)
            .OrderByDescending(rt => rt.ExpiresAt)
            .Skip(5)
            .ToListAsync();
    
        if (oldTokens.Any())
        {
            _dbContext.RefreshTokens.RemoveRange(oldTokens);
            await _dbContext.SaveChangesAsync();
        }
        
        var newRefreshTokenDto = await GenerateAndSaveNewRefreshTokenAsync(user.Id);
        var accessToken = await GenerateAccessTokenAsync(user);
        
        AppendRefreshTokenCookie(newRefreshTokenDto.Token, newRefreshTokenDto.ExpiresAt);

        return (accessToken, newRefreshTokenDto.Token);
    }
    
}