using FluentAssertions;
using InternshipTask.Domain.Models.Enums;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Repositories.Exceptions;
using InternshipTask.IntegrationTests.Fakers;
using InternshipTask.IntegrationTests.Fixtures;
using Xunit;

namespace InternshipTask.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class GameRepositoryTests
{
    private readonly IGameRepository _gameRepository;

    public GameRepositoryTests(TestFixture fixture)
    {
        _gameRepository = fixture.GameRepository;
    }

    [Fact]
    public async Task CreateGame_Should_ReturnIdGame()
    {
        // Arrange
        var game = GameModelFaker.Generate();
        
        // Act
        var result = await _gameRepository.CreateGameAsync(game);
        
        // Assert
        result.Should().Be(game.Id);
    }
    
    [Fact]
    public async Task GetGameByIdAsync_Should_ReturnGame()
    {
        // Arrange
        var game = GameModelFaker.Generate();
        await _gameRepository.CreateGameAsync(game);

        // Act
        var result = await _gameRepository.GetGameByIdAsync(game.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(game, x =>
            x.Excluding(f => f.GameFinishedAt));
    }
    
    [Fact]
    public async Task UpdateGameAsync_Should_UpdateGameSuccessfully()
    {
        // Arrange
        var game = GameModelFaker.Generate();
        await _gameRepository.CreateGameAsync(game);
        var updateGame = game.
            WithStatus(GameStatus.Win).
            WithTimeFinished(DateTime.UtcNow);
        
        // Act
        await _gameRepository.UpdateGameAsync(updateGame);

        // Assert
        var result = await _gameRepository.GetGameByIdAsync(game.Id);
        result!.Status.Should().Be(GameStatus.Win);
        result.GameFinishedAt.Should().NotBeNull();
    }
    
    
    [Fact]
    public async Task UpdateGameAsync_ShouldThrow_WhenGameNotExists()
    {
        // Arrange
        var game = GameModelFaker.Generate();

        // Act
        var act = async () => await _gameRepository.UpdateGameAsync(game);

        // Assert
        await act.Should().ThrowAsync<FailedGameUpdateException>();
    }
}