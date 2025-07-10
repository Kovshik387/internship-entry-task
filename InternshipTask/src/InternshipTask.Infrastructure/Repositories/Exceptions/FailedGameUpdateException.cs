namespace InternshipTask.Infrastructure.Repositories.Exceptions;

public class FailedGameUpdateException : Exception
{
    public FailedGameUpdateException(string? message) : base(message) { }
}