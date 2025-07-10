namespace InternshipTask.IntegrationTests.Creators;

public static class Create
{
    private static readonly Random StaticRandom = new();
    
    public static int RandomInt(int min, int max) => StaticRandom.Next(min, max);
}