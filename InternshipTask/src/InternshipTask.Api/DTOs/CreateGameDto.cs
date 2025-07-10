namespace InternshipTask.Api.DTOs;

public record CreateGameDto
{
    public required Guid PlayerIdX { get; init; }
    public required Guid PlayerIdO { get; init; }
}