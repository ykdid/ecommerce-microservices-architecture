using MediatR;

namespace Auth.Application.Features.Auth.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) 
    : IRequest<string>;