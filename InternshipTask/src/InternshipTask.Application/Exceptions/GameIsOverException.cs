namespace InternshipTask.Application.Exceptions;

public class GameIsOverException : GameException
{
    public GameIsOverException(string? message) : base(message) { }
}