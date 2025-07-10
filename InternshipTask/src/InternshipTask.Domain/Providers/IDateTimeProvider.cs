namespace InternshipTask.Domain.Providers;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}