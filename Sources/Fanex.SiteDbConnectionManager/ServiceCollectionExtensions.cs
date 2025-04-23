using Microsoft.Extensions.DependencyInjection;

namespace Fanex.SiteDbConnectionManager.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSiteDbConnectionManager(this IServiceCollection services)
    {
        services.AddTransient<SiteDbService, SiteDbServiceImp>();

        return services;
    }
}