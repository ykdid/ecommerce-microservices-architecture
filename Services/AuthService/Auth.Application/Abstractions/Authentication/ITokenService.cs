namespace Auth.Application.Abstractions.Authentication;

public interface ITokenService
{
    string GenerateToken(string userId, string email, IList<string> roles);
}