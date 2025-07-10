namespace InternshipTask.Application.Exceptions;

public class InvalidRulesException : GameException
{
    public InvalidRulesException(string? message) : base(message) { }
}