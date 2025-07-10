using InternshipTask.Application.Enums;

namespace InternshipTask.Application.DTOs;

public record CompleteMoveDto(
    string? Message,
    GameStatus Status,
    string ETag
    );