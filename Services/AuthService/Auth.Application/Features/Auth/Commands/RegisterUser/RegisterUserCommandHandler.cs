using Auth.Application.Abstractions.Authentication;
using Auth.Application.Abstractions.Identity;
using MediatR;

namespace Auth.Application.Features.Auth.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUserService _userService;

    public RegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, token, errors) = 
            await _userService.RegisterAsync(request.FullName, request.Email, request.Password);

        if (!succeeded)
            throw new Exception(string.Join(" | ", errors));

        return token;
    }
}