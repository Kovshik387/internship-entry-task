using InternshipTask.Application.DTOs;

namespace InternshipTask.Application.Interfaces;

public interface IGameService
{
    Task<Guid> CreateGameAsync(CreateGameDto dto, CancellationToken cancellationToken = default);

    Task<GameStateDto> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default);

    Task<CompleteMoveDto> MakeMoveAsync(MakeMoveDto dto, CancellationToken cancellationToken = default);
}