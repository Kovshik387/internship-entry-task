namespace InternshipTask.Domain.Models;

public record MoveModel
{
    public Guid Id { get; init; }
    public Guid GameId { get; init; }
    public Guid PlayerId { get; init; }
    public int Row { get; init; }
    public int Col { get; init; }
    public string Letter { get; init; } = string.Empty;
    public DateTime MoveTime { get; init; }
    public int MoveCount { get; init; }
    public string ETag { get; init; } = string.Empty;
}