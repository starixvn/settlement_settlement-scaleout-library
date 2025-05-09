using Fanex.SiteDbConnectionManager.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.SiteDbConnectionManager.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSiteDbConnectionManagerServices(this IServiceCollection services)
    {
        services.AddTransient<SiteDbService, SiteDbServiceImp>();
        services.AddTransient<SiteDbClientService, SiteDbClientServiceImp>();
        services.AddTransient<SiteDbPendingCommandService, SiteDbPendingCommandServiceImp>();

        return services;
    }
}