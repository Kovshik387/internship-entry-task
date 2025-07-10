using Dapper;
using InternshipTask.Domain.Models;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Common;
using InternshipTask.Infrastructure.Repositories.Exceptions;
using InternshipTask.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace InternshipTask.Infrastructure.Repositories;

public class MoveRepository : PgRepository, IMoveRepository
{
    public MoveRepository(IOptions<DbOptions> options) : base(options) { }

    public async Task<IReadOnlyList<MoveModel>> GetMovesByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        select id,
               game_id,
               player_id,
               row,
               col,
               letter,
               move_time,
               move_count,
               etag
          from moves
         where game_id = @GameId
         order by move_count;
";
        var cmd = new CommandDefinition(
            sql,
            new
            {
                GameId = gameId
            },
            cancellationToken: cancellationToken
        );
        
        await using var connection = await OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<MoveModel>(cmd))
            .ToList()
            .AsReadOnly();
    }

    public async Task MakeMoveAsync(MoveModel move, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        insert into moves (id, game_id, player_id, row, col, letter, move_time, move_count, etag)
        values (@Id, @GameId, @PlayerId, @Row, @Col, @Letter, @MoveTime, @MoveCount, @ETag);
";
        
        var cmd = new CommandDefinition(
            sql,
            new
            {
                Id = move.Id,
                GameId = move.GameId,
                PlayerId = move.PlayerId,
                Row = move.Row,
                Col = move.Col,
                Letter = move.Letter,
                MoveTime = move.MoveTime,
                MoveCount = move.MoveCount,
                ETag = move.ETag
            },
            cancellationToken: cancellationToken
        );
        
        await using var connection = await OpenConnectionAsync(cancellationToken);
        var count = await connection.ExecuteAsync(cmd);

        if (count < 1) throw new FailedMoveAddException("Failed add entity");
    }

    public async Task<MoveModel?> GetMoveByETagAsync(Guid gameId, string eTag, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        select id,
               game_id,
               player_id,
               row,
               col,
               letter,
               move_time,
               move_count,
               etag
          from moves
         where game_id = @GameId
           and etag = @ETag
         limit 1;
";
        
        var cmd = new CommandDefinition(
            sql,
            new
            {
                GameId = gameId, ETag = eTag
            },
            cancellationToken: cancellationToken
        );

        await using var connection = await OpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<MoveModel>(cmd);
    }
}