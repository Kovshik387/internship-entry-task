namespace InternshipTask.Infrastructure.Repositories.Exceptions;

public class FailedMoveAddException : Exception 
{
    public FailedMoveAddException(string? message) : base(message) { }
}