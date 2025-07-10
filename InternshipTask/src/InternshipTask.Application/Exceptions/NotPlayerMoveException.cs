namespace InternshipTask.Application.Exceptions;

public class NotPlayerMoveException : GameException
{
    public NotPlayerMoveException(string? message) : base(message)  { }
}