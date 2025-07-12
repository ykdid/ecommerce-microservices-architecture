using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

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

    public async Task<(bool Succeeded, string Token, string[] Errors)> RegisterAsync(string fullName, string email, string password)
    {
        var user = new ApplicationUser
        {
            FullName = fullName,
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (false, string.Empty, result.Errors.Select(e => e.Description).ToArray());

        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        return (true, token, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string Token, string Error)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (false, string.Empty, "User not found.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return (false, string.Empty, "Password is Wrong.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        return (true, token, string.Empty);
    }
}