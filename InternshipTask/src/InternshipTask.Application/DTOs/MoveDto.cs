namespace InternshipTask.Application.DTOs;

public record MoveDto(
    Guid Id,
    Guid GameId,
    Guid PlayerId,
    int Row,
    int Column,
    string Letter,
    DateTime MoveTime,
    int MoveCount,
    string ETag
    );