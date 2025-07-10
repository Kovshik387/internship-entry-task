using InternshipTask.Application.Enums;

namespace InternshipTask.Application.DTOs;

public record GameStateDto(
    Guid GameId,
    int  BoardSize,
    int  WinLength,
    GameStatus Status,
    Guid CurrentPlayerId,
    Guid PlayerIdX,
    Guid PlayerIdO,
    IReadOnlyList<MoveDto> Moves
    );