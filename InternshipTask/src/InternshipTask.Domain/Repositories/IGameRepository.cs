using InternshipTask.Domain.Models;

namespace InternshipTask.Domain.Repositories;

public interface IGameRepository
{
    Task<GameModel?> GetGameByIdAsync(Guid gameId, CancellationToken cancellationToken = default);
    
    Task<Guid> CreateGameAsync(GameModel game, CancellationToken cancellationToken = default);
    
    Task UpdateGameAsync(GameModel game, CancellationToken cancellationToken = default);
}