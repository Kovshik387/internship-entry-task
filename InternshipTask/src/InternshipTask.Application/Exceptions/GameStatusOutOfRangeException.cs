namespace InternshipTask.Application.Exceptions;

public class GameStatusOutOfRangeException : ArgumentOutOfRangeException
{
    public GameStatusOutOfRangeException(string? paramName, string? message) : base(message, paramName) { }
}