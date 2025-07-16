using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using MediatR;

namespace Auth.Application.Features.Auth.Commands.LoginUser;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, (string Token, string RefreshToken)>
{
    private readonly IUserService _userService;

    public LoginUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<(string Token, string RefreshToken)> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, token, refreshToken, error) = await _userService.LoginAsync(request.Email, request.Password);

        if (!succeeded)
            throw new Exception(error);

        return (token, refreshToken);
    }
}