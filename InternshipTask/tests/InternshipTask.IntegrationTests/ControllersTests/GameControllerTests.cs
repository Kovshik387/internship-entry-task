using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InternshipTask.Application.DTOs;
using InternshipTask.Application.Extensions.Converters;
using InternshipTask.IntegrationTests.Fakers;
using InternshipTask.IntegrationTests.Fixtures;
using Xunit;

namespace InternshipTask.IntegrationTests.ControllersTests;
[CollectionDefinition(nameof(TestFixture))]
public class GameControllerTests : IClassFixture<TestFixture>, IClassFixture<ApiFixture<Program>>
{
    private readonly HttpClient _client;
    
    private const string Root = "/api/games/";
    
    public GameControllerTests(ApiFixture<Program> fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateGame_Should_ReturnGameId()
    {
        // Arrange
        var dtoPlayers = CreateGameDtoFaker.Generate();
        
        // Act
        var response = await _client.PostAsJsonAsync(Root + "create", dtoPlayers);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var gameId = await response.Content.ReadFromJsonAsync<Guid>();
        gameId.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetGame_Should_ReturnGameState()
    {
        // Arrange
        var dtoPlayers = CreateGameDtoFaker.Generate();
        var createResponse = await (await _client.PostAsJsonAsync(Root + "create", dtoPlayers))
            .Content.ReadFromJsonAsync<Guid>();

        // Act
        var response = await _client.GetAsync(Root + $"{createResponse}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var game = await response.Content.ReadFromJsonAsync<GameStateDto>();
        game.Should().NotBeNull();
        game!.GameId.Should().Be(createResponse);
        game.PlayerIdX.Should().Be(dtoPlayers.PlayerIdX);
        game.PlayerIdO.Should().Be(dtoPlayers.PlayerIdO);
    }

    [Fact]
    public async Task GetGame_Should_ReturnNotFound()
    {
        // Arrange
        var guid = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync(Root + $"{guid}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task InvalidData_Should_ReturnBadRequest()
    {
        // Arrange
        var dtoPlayers = new
        {
            PlayerIdX = Guid.NewGuid(),
        };
        
        // Act
        var response = await _client.PostAsJsonAsync(Root + "create", dtoPlayers);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task MakeMove_Should_ReturnOkAndSetsETag()
    {
        // Arrange
        var dtoPlayers = CreateGameDtoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync(Root + "create", dtoPlayers);
        var gameId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var moveDto = MoveModelFaker.Generate().First()
            .WithGameId(gameId)
            .WithPlayerId(dtoPlayers.PlayerIdX)
            .ToDto();

        // Act
        var request = new HttpRequestMessage(HttpMethod.Post, Root + $"{gameId}/moves")
        {
            Content = JsonContent.Create(moveDto)
        };

        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.ETag.Should().NotBeNull();
    }

    [Fact]
    public async Task MakeRepeatMove_Should_ReturnOkAndETag()
    {
        // Arrange
        var dtoPlayers = CreateGameDtoFaker.Generate();
        var createResponse = await _client.PostAsJsonAsync(Root + "create", dtoPlayers);
        var gameId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var moveDto = MoveModelFaker.Generate().First()
            .WithGameId(gameId)
            .WithPlayerId(dtoPlayers.PlayerIdX)
            .ToDto();
        var request = new HttpRequestMessage(HttpMethod.Post, Root + $"{gameId}/moves")
        {
            Content = JsonContent.Create(moveDto)
        };
        var response = await _client.SendAsync(request);
        var eTag = response.Headers.ETag!.Tag.Trim('"');
        
        // Act
        var secondRequest = new HttpRequestMessage(HttpMethod.Post, Root + $"{gameId}/moves")
        {
            Content = JsonContent.Create(moveDto)
        };
        secondRequest.Headers.IfMatch.ParseAdd($"\"{eTag}\"");
        var responseWithETag = await _client.SendAsync(secondRequest);
        
        // Assert
        responseWithETag.StatusCode.Should().Be(HttpStatusCode.OK);
        var tag = responseWithETag.Headers.ETag!.Tag.Trim('"');
        tag.Should().BeEquivalentTo(eTag);
    }
}