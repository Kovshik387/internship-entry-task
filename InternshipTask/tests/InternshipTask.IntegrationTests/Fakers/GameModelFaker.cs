using Bogus;
using InternshipTask.Domain.Models;
using InternshipTask.Domain.Models.Enums;

namespace InternshipTask.IntegrationTests.Fakers;

public static class GameModelFaker
{
    private static readonly Guid PlayerIdX = Guid.NewGuid();
    private const int BoardSize = 3;
    private const int WinLength = 3;

    private static readonly Faker<GameModel> Faker = new Faker<GameModel>()
        .RuleFor(x => x.Id, _ => Guid.NewGuid())
        .RuleFor(x => x.PlayerIdX, _ => PlayerIdX)
        .RuleFor(x => x.PlayerIdO, _ => Guid.NewGuid())
        .RuleFor(x => x.CurrentPlayerId, _ => PlayerIdX)
        .RuleFor(x => x.GameCreatedAt, f => f.Date.Recent())
        .RuleFor(x => x.Status, _ => GameStatus.NotOver)
        .RuleFor(x => x.BoardSize, _ => BoardSize)
        .RuleFor(x => x.WinLength, _ => WinLength)
        ;

    public static GameModel Generate()
    {
        return Faker.Generate();
    }

    public static GameModel WithBoardSizeWinLength(this GameModel src, int boardSize, int winLength) => src with
    {
        BoardSize = boardSize,
        WinLength = winLength
    };

    public static GameModel WithIdCurrentPlayer(this GameModel src, Guid currentPlayerId) 
        => src with { CurrentPlayerId = currentPlayerId };
    
    public static GameModel WithStatus(this GameModel src, GameStatus status) => src with { Status = status };
    
    public static GameModel WithId(this GameModel src, Guid id) => src with { Id = id };

    public static GameModel WithTimeFinished(this GameModel src, DateTime time) => src with { GameFinishedAt = time };
}