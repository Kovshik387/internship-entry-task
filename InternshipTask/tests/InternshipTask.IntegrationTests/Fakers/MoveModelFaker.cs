
using Bogus;
using InternshipTask.Domain.Models;
using InternshipTask.IntegrationTests.Creators;

namespace InternshipTask.IntegrationTests.Fakers;

public static class MoveModelFaker
{
    private static readonly object Lock = new();
    
    private const int BoardSize = 3 - 1;
    
    private static readonly Faker<MoveModel> Faker = new Faker<MoveModel>()
        .RuleFor(x => x.Id, _ => Guid.NewGuid())
        .RuleFor(x => x.GameId, _ => Guid.NewGuid())
        .RuleFor(x => x.PlayerId, _ => Guid.NewGuid())
        .RuleFor(x => x.Row, f => Create.RandomInt(0,BoardSize))
        .RuleFor(x => x.Col, f => Create.RandomInt(0,BoardSize))
        .RuleFor(x => x.Letter, f => f.PickRandom("X","O"))
        .RuleFor(x => x.MoveTime, f => f.Date.Recent())
        .RuleFor(x => x.MoveCount, f => f.Random.Int(1, 5))
        .RuleFor(x => x.ETag, f => f.Hashids.ToString())
        ;

    public static MoveModel[] Generate(int count = 1, Guid gameId = default)
    {
        lock (Lock)
        {
            return gameId == Guid.Empty 
                ? Faker.Generate(count).ToArray() 
                : Faker.Generate(count).Select(x => x.WithGameId(gameId)).ToArray();
        }
    }
    
    public static MoveModel WithGameId(this MoveModel src, Guid gameId) => src with { GameId = gameId };
    
    public static MoveModel WithPlayerId(this MoveModel src, Guid playerId) => src with { PlayerId = playerId };
}