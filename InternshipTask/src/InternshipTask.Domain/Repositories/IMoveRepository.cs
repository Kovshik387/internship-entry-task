using InternshipTask.Domain.Models;

namespace InternshipTask.Domain.Repositories;

public interface IMoveRepository
{
    Task<IReadOnlyList<MoveModel>> GetMovesByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default);
    
    Task MakeMoveAsync(MoveModel move, CancellationToken cancellationToken = default);
    
    Task<MoveModel?> GetMoveByETagAsync(Guid gameId, string eTag, CancellationToken cancellationToken = default);
}