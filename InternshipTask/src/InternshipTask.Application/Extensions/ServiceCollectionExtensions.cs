using InternshipTask.Application.Interfaces;
using InternshipTask.Application.Services;
using InternshipTask.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternshipTask.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(nameof(RuleOptions));
        services.Configure<RuleOptions>(settingsSection);
        
        services.AddTransient<IETagService, ETagService>();
        services.AddTransient<IGameRuleService, GameRuleService>();
        services.AddTransient<IGameService, GameService>();
        
        return services;
    }
}