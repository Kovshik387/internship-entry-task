using InternshipTask.Application.Enums;
using InternshipTask.Application.Exceptions;

namespace InternshipTask.Application.Extensions.Converters;

using ModelGameStatus = Domain.Models.Enums.GameStatus;

public static class GameStatusTypeConverter
{
    public static GameStatus ToDto(this ModelGameStatus model) => model switch
    {
        ModelGameStatus.Unknown => GameStatus.Unknown,
        ModelGameStatus.NotOver => GameStatus.NotOver,
        ModelGameStatus.Draw => GameStatus.Draw,
        ModelGameStatus.Win => GameStatus.Win,
        _ => throw new GameStatusOutOfRangeException(nameof(model), "Invalid game status")
    };  
    public static ModelGameStatus ToModel(this GameStatus dto) => dto switch
    {
        GameStatus.Unknown => ModelGameStatus.Unknown,
        GameStatus.NotOver => ModelGameStatus.NotOver,
        GameStatus.Draw => ModelGameStatus.Draw,
        GameStatus.Win => ModelGameStatus.Win,
        _ => throw new GameStatusOutOfRangeException(nameof(dto), "Invalid game status")
    };
}