using FluentAssertions;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Repositories.Exceptions;
using InternshipTask.IntegrationTests.Fakers;
using InternshipTask.IntegrationTests.Fixtures;
using Xunit;

namespace InternshipTask.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class MoveRepositoryTests
{
    private readonly IMoveRepository _moveRepository;

    public MoveRepositoryTests(TestFixture fixture)
    {
        _moveRepository = fixture.MoveRepository;
    }

    [Fact]
    public async Task MakeMoveAsync_Should_ReturnSuccess()
    {
        // Arrange
        var move = MoveModelFaker.Generate().First();
        
        // Act
        var act = async () => await _moveRepository.MakeMoveAsync(move);

        // Assert
        await act.Should().NotThrowAsync<FailedMoveAddException>();
    }
 
    [Fact]
    public async Task GetMovesByGameIdAsync_ShouldReturnAllMovesOrderedByCount()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var moves = MoveModelFaker.Generate(3, gameId);
        foreach (var move in moves)
            await _moveRepository.MakeMoveAsync(move);

        // Act
        var result = await _moveRepository.GetMovesByGameIdAsync(gameId);

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeInAscendingOrder(x => x.MoveCount);
    }
 
    [Fact]
    public async Task GetMoveByETagAsync_ShouldReturnCorrectMove()
    {
        // Arrange
        var move = MoveModelFaker.Generate().First();
        await _moveRepository.MakeMoveAsync(move);

        // Act
        var result = await _moveRepository.GetMoveByETagAsync(move.GameId, move.ETag);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(move);
    }
    
}