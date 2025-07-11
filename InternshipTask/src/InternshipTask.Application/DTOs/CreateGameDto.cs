namespace InternshipTask.Application.DTOs;

public record CreateGameDto
{
    public required Guid PlayerIdX { get; init; }
    public required Guid PlayerIdO { get; init; }
}