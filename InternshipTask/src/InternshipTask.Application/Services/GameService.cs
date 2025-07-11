using System.Transactions;
using InternshipTask.Application.DTOs;
using InternshipTask.Application.Exceptions;
using InternshipTask.Application.Extensions.Converters;
using InternshipTask.Application.Interfaces;
using InternshipTask.Application.Settings;
using InternshipTask.Domain.Models;
using InternshipTask.Domain.Models.Enums;
using InternshipTask.Domain.Providers;
using InternshipTask.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InternshipTask.Application.Services;

public class GameService : IGameService
{
    private readonly ILogger<GameService> _logger;
    private readonly RuleOptions _ruleOptions;
    
    private readonly IGameRepository _gameRepository;
    private readonly IMoveRepository _moveRepository;

    private readonly IETagService _etagService;
    private readonly IGameRuleService _gameRuleService;
    
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public GameService(ILogger<GameService> logger, IOptions<RuleOptions> ruleOptions, IGameRepository gameRepository,
        IMoveRepository moveRepository, IGameRuleService gameRuleService, IETagService eTagService, 
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _ruleOptions = ruleOptions.Value;
        _gameRepository = gameRepository;
        _moveRepository = moveRepository;
        _gameRuleService = gameRuleService;
        _etagService = eTagService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> CreateGameAsync(CreateGameDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (_ruleOptions.Size < 3)
        {
            _logger.LogError("Размер поля должен быть больше или равен 3");
            throw new InvalidRulesException("Размер поля должен быть больше или равен 3");
        }

        var gameModel = new GameModel()
        {
            Id = Guid.NewGuid(),
            PlayerIdO = dto.PlayerIdO,
            PlayerIdX = dto.PlayerIdX,
            WinLength = _ruleOptions.WinLength,
            BoardSize = _ruleOptions.Size,
            CurrentPlayerId = dto.PlayerIdX,
            GameCreatedAt = _dateTimeProvider.Now.UtcDateTime,
            Status = GameStatus.NotOver
        };

        var id = await _gameRepository.CreateGameAsync(gameModel, cancellationToken);

        return id;
    }

    public async Task<GameStateDto> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var game = await _gameRepository.GetGameByIdAsync(gameId, cancellationToken);
        if (game is null) throw new GameNotFoundException("Игра не найдена");
        
        var gameMoves = (await _moveRepository.GetMovesByGameIdAsync(gameId,
                cancellationToken))
            .OrderBy(x => x.MoveCount)
            .ToList()
            .AsReadOnly();

        return new GameStateDto(
            game.Id, 
            game.BoardSize,
            game.WinLength,
            game.Status.ToDto(),
            game.CurrentPlayerId,
            game.PlayerIdX,
            game.PlayerIdO,
            gameMoves.Select(x => x.ToDto()).ToList().AsReadOnly()
            );
    }

    public async Task<CompleteMoveDto> MakeMoveAsync(MakeMoveDto dto,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!string.IsNullOrEmpty(dto.ETag))
        {
            var exist = await _moveRepository.GetMoveByETagAsync(dto.GameId, dto.ETag.Trim('"'), cancellationToken);
            if (exist is not null) throw new ETagException("Ход был выполнен", dto.ETag);
        }
        else dto.ETag = _etagService.GenerateETag(dto);
        
        var game = await _gameRepository.GetGameByIdAsync(dto.GameId, cancellationToken);

        if (game is null) throw new GameNotFoundException("Игра не найдена");
        
        if (game.Status != GameStatus.NotOver)
            throw new GameIsOverException("Игра закончена"); 
        
        if (game.CurrentPlayerId != dto.PlayerId)
            throw new NotPlayerMoveException("Ходит другой игрок");
        
        var moves = (await _moveRepository.GetMovesByGameIdAsync(dto.GameId, cancellationToken)).ToList();
        var moveCount = moves.Count + 1;
        
        if (moves.Any(x => x.Row == dto.Row && x.Col == dto.Column))
            throw new CellIsOccupiedException("Клетка занята");

        var isPlayerX = dto.PlayerId == game.PlayerIdX;
        var letter = isPlayerX ? "X" : "O";
        
        if (moveCount % 3 == 0 && Random.Shared.NextDouble() < 0.1)
        {
            _logger.LogInformation($"Игра: {game.Id} ход: {moveCount} символ изменён на {letter}");
            letter = isPlayerX ? "O" : "X";
        }

        var move = new MoveDto(
            Guid.NewGuid(),
            game.Id,
            dto.PlayerId,
            dto.Row,
            dto.Column,
            letter,
            _dateTimeProvider.Now.UtcDateTime,
            moveCount,
            dto.ETag
        );
        
        var moveModel = move.ToModel();
        moves.Add(moveModel);
        
        var status = _gameRuleService.CheckStatus(moves, _ruleOptions.Size, _ruleOptions.WinLength);
        
        var newStateGame = game with
        {
            Status = status.ToModel(),
            CurrentPlayerId = move.PlayerId == game.PlayerIdX ? game.PlayerIdO : game.PlayerIdX
        };
        
        using var transactionScope = CreateTransactionScope();
        
        await _moveRepository.MakeMoveAsync(moveModel, cancellationToken);
        await _gameRepository.UpdateGameAsync(newStateGame, cancellationToken);
        
        transactionScope.Complete();
        
        return new CompleteMoveDto(
            "Ход был выполнен",
            newStateGame.Status.ToDto(),
            dto.ETag
        );
    }
    
    private static TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions 
            { 
                IsolationLevel = level, 
                Timeout = TimeSpan.FromSeconds(5) 
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}