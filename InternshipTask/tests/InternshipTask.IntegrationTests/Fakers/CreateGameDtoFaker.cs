using Bogus;
using InternshipTask.Application.DTOs;

namespace InternshipTask.IntegrationTests.Fakers;

public static class CreateGameDtoFaker
{
    private static readonly Faker<CreateGameDto> Faker = new Faker<CreateGameDto>()
        .RuleFor(x => x.PlayerIdX, _ => Guid.NewGuid())
        .RuleFor(x => x.PlayerIdO, _ => Guid.NewGuid());
    
    public static CreateGameDto Generate()
    {
        return Faker.Generate();
    }
}