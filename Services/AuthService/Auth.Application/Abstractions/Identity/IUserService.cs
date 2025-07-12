namespace Auth.Application.Abstractions.Identity;

public interface IUserService
{
    Task<(bool Succeeded, string Token, string[] Errors)> RegisterAsync(string fullName, string email, string password);
    Task<(bool Succeeded, string Token, string Error)> LoginAsync(string email, string password);
}