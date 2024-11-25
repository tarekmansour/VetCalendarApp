using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public record CreatePatientRequest
{
    [Required]
    public required string Name { get; init; }
    [Required]
    public required string Species { get; init; }
    [Required]
    public required DateTime DateOfBirth { get; init; }
};

