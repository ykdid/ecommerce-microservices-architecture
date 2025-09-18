namespace Order.Application.DTOs;

public sealed record AddressDto(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
);