using MediatR;

namespace Auth.Application.Features.Auth.Commands.RegisterUser;


public sealed record RegisterUserCommand(string FullName, string Email, string Password) 
    : IRequest<(string Token, string RefreshToken)>;