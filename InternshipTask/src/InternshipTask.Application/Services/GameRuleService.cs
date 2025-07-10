using InternshipTask.Application.Enums;
using InternshipTask.Application.Interfaces;
using InternshipTask.Domain.Models;

namespace InternshipTask.Application.Services;

public class GameRuleService : IGameRuleService
{
    public GameStatus CheckStatus(IReadOnlyList<MoveModel> moves, int boardSize, int winLength)
    {
        var board = new string[boardSize, boardSize];

        foreach (var move in moves)
        {
            board[move.Row, move.Col] = move.Letter;
        }

        if (moves.Any(move => IsWinningMove(board, move.Row, move.Col, move.Letter, winLength)))
        {
            return GameStatus.Win;
        }

        return moves.Count == boardSize * boardSize ? GameStatus.Draw : GameStatus.NotOver;
    }
    
    private bool IsWinningMove(string[,] board, int row, int col, string symbol, int winLength)
    {
        return CheckDirection(board, row, col, symbol, 0, 1) + 
               CheckDirection(board, row, col, symbol, 0, -1) - 1 >= winLength ||
               CheckDirection(board, row, col, symbol, 1, 0) + 
               CheckDirection(board, row, col, symbol, -1, 0) - 1 >= winLength ||
               CheckDirection(board, row, col, symbol, 1, 1) + 
               CheckDirection(board, row, col, symbol, -1, -1) - 1 >= winLength ||
               CheckDirection(board, row, col, symbol, 1, -1) + 
               CheckDirection(board, row, col, symbol, -1, 1) - 1 >= winLength;
    }

    private int CheckDirection(string[,] board, int row, int col, string symbol, int dx, int dy)
    {
        var count = 0;
        var n = board.GetLength(0);

        while (row >= 0 && row < n && col >= 0 && col < n && board[row, col] == symbol)
        {
            count++;
            row += dx;
            col += dy;
        }

        return count;
    }
}