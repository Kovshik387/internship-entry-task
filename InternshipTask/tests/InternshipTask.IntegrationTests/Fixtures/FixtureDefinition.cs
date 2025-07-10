using Xunit;

namespace InternshipTask.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public class FixtureDefinition : ICollectionFixture<TestFixture>
{
    
}