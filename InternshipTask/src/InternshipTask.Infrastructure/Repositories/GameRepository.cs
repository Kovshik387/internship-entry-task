using Dapper;
using InternshipTask.Domain.Models;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Common;
using InternshipTask.Infrastructure.Repositories.Exceptions;
using InternshipTask.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace InternshipTask.Infrastructure.Repositories;

public sealed class GameRepository : PgRepository, IGameRepository
{
    public GameRepository(IOptions<DbOptions> options) : base(options) { }

    public async Task<GameModel?> GetGameByIdAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        select id
             , board_size
             , win_length
             , status
             , player_id_x
             , player_id_o
             , current_player_id
             , game_created_at
             , game_finished_at
          from games
         where id = @Id
";

        await using var connection = await OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            sql,
            new
            {
                Id = gameId
            },
            cancellationToken: cancellationToken
        );

        var model = await connection.QueryAsync<GameModel>(cmd);

        return model.FirstOrDefault();
    }

    public async Task<Guid> CreateGameAsync(GameModel game, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        insert into games as g (id, board_size, win_length, status, player_id_x, player_id_o, current_player_id, game_created_at)
        values (@Id, @BoardSize, @WinLength, @Status, @PlayerIdX, @PlayerIdO, @CurrentPlayerId, @GameCreatedAt)
     returning id 
";
        
        await using var connection = await OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            sql,
            new
            {
                Id = game.Id,
                BoardSize = game.BoardSize,
                WinLength = game.WinLength,
                Status = game.Status,
                PlayerIdX = game.PlayerIdX,
                PlayerIdO = game.PlayerIdO,
                CurrentPlayerId = game.CurrentPlayerId,
                GameCreatedAt = game.GameCreatedAt
            },
            cancellationToken: cancellationToken
        );

        var id = await connection.QueryAsync<Guid>(cmd);

        return game.Id;
    }

    public async Task UpdateGameAsync(GameModel game, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        update games set
                         board_size = @BoardSize,
                         win_length = @WinLength,
                         status = @Status,
                         current_player_id = @CurrentPlayerId,
                         game_finished_at = @GameFinishedAt
         where id = @Id
";
        var cmd = new CommandDefinition(sql,
            new
            {
                Id = game.Id,
                BoardSize = game.BoardSize,
                WinLength = game.WinLength,
                Status = game.Status,
                CurrentPlayerId = game.CurrentPlayerId,
                GameFinishedAt = game.GameFinishedAt
            },
            cancellationToken: cancellationToken);
        var connection = await OpenConnectionAsync(cancellationToken);
        
        var updateCount = await connection.ExecuteAsync(cmd);

        if (updateCount < 1) throw new FailedGameUpdateException("Failed find entity for update");
    }
}