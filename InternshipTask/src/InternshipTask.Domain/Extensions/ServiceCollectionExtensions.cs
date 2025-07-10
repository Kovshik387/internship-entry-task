using InternshipTask.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace InternshipTask.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}