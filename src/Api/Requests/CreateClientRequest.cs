using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record CreateClientRequest
{
    [Required]
    public required string Name { get; init; }
    [Required]
    public required string PhoneNumber { get; init; }
    [Required]
    public required string Email { get; init; }
};
