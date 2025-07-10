namespace InternshipTask.Application.Exceptions;

public class GameNotFoundException : GameException
{
    public GameNotFoundException(string? message) : base(message) { }
}