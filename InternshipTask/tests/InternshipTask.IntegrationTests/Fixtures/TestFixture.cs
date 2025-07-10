using FluentMigrator.Runner;
using InternshipTask.Application.Extensions;
using InternshipTask.Domain.Extensions;
using InternshipTask.Domain.Providers;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;

namespace InternshipTask.IntegrationTests.Fixtures;

public class TestFixture
{
    public IGameRepository GameRepository { get; }
    public IMoveRepository MoveRepository { get; }
    
    public Mock<IDateTimeProvider> DateTimeProvider { get; }

    public TestFixture()
    {
        DateTimeProvider = new Mock<IDateTimeProvider>();
        DateTimeProvider.Setup(x => x.Now).Returns(DateTime.Now);
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddApplicationLayer(config);
                services.AddInfrastructure(config);

                services.AddProviders();
                services.Replace(new ServiceDescriptor(typeof(IDateTimeProvider), DateTimeProvider.Object));
            })
            .Build();
            
        ClearDatabase(host);
        host.MigrateUp();
        
        var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        GameRepository = serviceProvider.GetRequiredService<IGameRepository>();
        MoveRepository = serviceProvider.GetRequiredService<IMoveRepository>();
        
        FluentAssertionOptions.UseDefaultPrecision();
    }
    
    private static void ClearDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(0);
    }
}