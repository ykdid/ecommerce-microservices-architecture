using MediatR;

namespace Auth.Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand : IRequest<string>;