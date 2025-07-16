namespace Auth.Application.DTOs;

public sealed record UserDto(
    string Id,
    string Email,
    string UserName,
    IList<string> Roles);