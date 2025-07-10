using FluentMigrator.Runner;
using InternshipTask.Domain.Providers;
using InternshipTask.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace InternshipTask.IntegrationTests.Fixtures;

public class ApiFixture<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public HttpClient Client { get; init; }
    private Mock<IDateTimeProvider> DateTimeProviderMock { get; } = new();

    public ApiFixture()
    {
        Client = CreateClient();
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddJsonFile("appsettings.json");
        });
        
        builder.ConfigureServices(x =>
        {
            DateTimeProviderMock.Setup(f => f.Now).Returns(DateTime.UtcNow);
            x.Replace(ServiceDescriptor.Singleton(DateTimeProviderMock.Object));
        });
        
        var host = base.CreateHost(builder);
        
        // ClearDatabase(host);
        // host.MigrateUp();

        return host;
    }
    
    private static void ClearDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(0);
    }
}