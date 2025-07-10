using InternshipTask.Domain.Models.Enums;

namespace InternshipTask.Domain.Models;

public record GameModel
{
    public Guid Id { get; init; }
    public int BoardSize { get; init; }
    public int WinLength { get; init; }
    public GameStatus Status { get; init; }
    public Guid PlayerIdX { get; init; }
    public Guid PlayerIdO { get; init; }
    public Guid CurrentPlayerId { get; init; }
    public DateTime GameCreatedAt { get; init; }
    public DateTime? GameFinishedAt { get; init; }
}