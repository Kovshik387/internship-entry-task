using FluentMigrator;
using InternshipTask.Infrastructure.Common;

namespace InternshipTask.Infrastructure.Migrations;
[Migration(1)]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
        create table games (
            id                  uuid primary key not null,
            board_size          int not null default 3,
            win_length          int not null default 3,
            status              int not null default 0,
            player_id_x         uuid not null,
            player_id_o         uuid not null,
            current_player_id   uuid not null,
            game_created_at     timestamp with time zone not null,
            game_finished_at    timestamp with time zone null
        );

        create table moves (
            id                  uuid primary key not null,
            game_id             uuid not null,
            player_id           uuid not null,
            row                 int not null,
            col              int not null,
            letter              text not null,
            move_time           timestamp with time zone not null,
            move_count          int not null default 0,
            etag                text not null
        );
";

    protected override string GetDownSql(IServiceProvider services) => @"
        drop table games;
        drop table moves;
";
}