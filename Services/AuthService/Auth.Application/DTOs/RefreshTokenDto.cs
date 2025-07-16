namespace Auth.Application.DTOs;

public record RefreshTokenDto(string Token, DateTime ExpiresAt);