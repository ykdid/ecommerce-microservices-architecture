using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}