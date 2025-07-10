namespace InternshipTask.Application.Exceptions;

public abstract class GameException : Exception
{
    protected GameException(string? message) : base(message) { }
}