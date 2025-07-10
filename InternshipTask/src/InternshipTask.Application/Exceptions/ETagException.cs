namespace InternshipTask.Application.Exceptions;

public class ETagException : Exception
{
    public string ETag { get; }
    
    public ETagException(string? message, string eTag) : base(message) { ETag = eTag; }
}