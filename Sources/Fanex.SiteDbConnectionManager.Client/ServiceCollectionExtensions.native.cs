using Fanex.SiteDbConnectionManager.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.SiteDbConnectionManager.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSiteDbConnectionManager(this IServiceCollection services)
    {
        services.AddTransient<SiteDbService, SiteDbServiceImp>();
        services.AddTransient<SiteDbClientService, SiteDbClientServiceImp>();

        return services;
    }
}