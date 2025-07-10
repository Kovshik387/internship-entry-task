namespace InternshipTask.Application.Exceptions;

public class CellIsOccupiedException : GameException
{
    public CellIsOccupiedException(string? message) : base(message) { }
}