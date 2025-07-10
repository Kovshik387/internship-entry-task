namespace InternshipTask.Application.DTOs;

public record MakeMoveDto
{
    public Guid GameId { get; init; }
    public Guid PlayerId { get; init; }
    public int Row { get; init; }
    public int Column { get; init; }
    public string? ETag { get; set; }
}