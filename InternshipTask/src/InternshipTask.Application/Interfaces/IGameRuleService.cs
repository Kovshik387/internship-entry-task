using InternshipTask.Application.Enums;
using InternshipTask.Domain.Models;

namespace InternshipTask.Application.Interfaces;

public interface IGameRuleService
{
    GameStatus CheckStatus(IReadOnlyList<MoveModel> moves, int boardSize, int winLength);
}