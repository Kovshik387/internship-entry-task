using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using InternshipTask.Domain.Repositories;
using InternshipTask.Infrastructure.Common;
using InternshipTask.Infrastructure.Repositories;
using InternshipTask.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternshipTask.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(nameof(DbOptions));
        services.Configure<DbOptions>(settingsSection);
        
        var settings = settingsSection.Get<DbOptions>();
        if (settings == null) throw new NullReferenceException("Db settings not found");

        services.AddSingleton<IGameRepository, GameRepository>();
        services.AddSingleton<IMoveRepository, MoveRepository>();
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddFluentMigrator(settings.ConnectionString, typeof(SqlMigration).Assembly);
        
        return services;
    }

    private static void AddFluentMigrator(this IServiceCollection services,
        string connectionString,
        Assembly assembly)
    {
        services
            .AddFluentMigratorCore()
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .ConfigureRunner(x => x
                .AddPostgres()
                .ScanIn(assembly).For.Migrations())
            .AddOptions<ProcessorOptions>()
            .Configure(x =>
            {
                x.ProviderSwitches = "Force Quote=false";
                x.Timeout = TimeSpan.FromMinutes(10);
                x.ConnectionString = connectionString;
            });
    }
}