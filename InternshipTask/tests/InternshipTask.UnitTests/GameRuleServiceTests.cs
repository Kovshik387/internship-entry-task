using FluentAssertions;
using InternshipTask.Application.Enums;
using InternshipTask.Application.Interfaces;
using InternshipTask.Application.Services;
using InternshipTask.Domain.Models;
using InternshipTask.UnitTests.Creators;
using Xunit;

namespace InternshipTask.UnitTests;

public class GameRuleServiceTests
{
    private readonly IGameRuleService _gameRuleService = new GameRuleService();
    
    [Fact]
    public void CheckStatusInHorizontal_Should_ReturnWin()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 0, "X"),
            Create.CreateMove(0, 1, "X"),
            Create.CreateMove(0, 2, "X")
        };

        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.Win);
    }

    [Fact]
    public void CheckStatusInVertical_Should_ReturnWin()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 0, "O"),
            Create.CreateMove(1, 0, "O"),
            Create.CreateMove(2, 0, "O")
        };

        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.Win);
    }

    [Fact]
    public void CheckStatusInDiagonal_Should_ReturnWin()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 0, "X"),
            Create.CreateMove(1, 1, "X"),
         Create.   CreateMove(2, 2, "X")
        };
        
        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.Win);
    }

    [Fact]
    public void CheckStatus_Win_AntiDiagonal()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 2, "O"),
            Create.CreateMove(1, 1, "O"),
            Create.CreateMove(2, 0, "O")
        };

        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.Win);
    }

    [Fact]
    public void CheckStatus_Draw()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 0, "X"), Create.CreateMove(0, 1, "O"), Create.CreateMove(0, 2, "X"),
            Create.CreateMove(1, 0, "X"), Create.CreateMove(1, 1, "O"), Create.CreateMove(1, 2, "X"),
            Create.CreateMove(2, 0, "O"), Create.CreateMove(2, 1, "X"), Create.CreateMove(2, 2, "O")
        };
        
        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.Draw);
    }

    [Fact]
    public void CheckStatus_NotOver()
    {
        // Arrange
        var moves = new List<MoveModel>
        {
            Create.CreateMove(0, 0, "X"),
            Create.CreateMove(0, 1, "O")
        };
        
        // Act
        var result = _gameRuleService.CheckStatus(moves, 3, 3);
        
        // Assert
        result.Should().Be(GameStatus.NotOver);
    }
}