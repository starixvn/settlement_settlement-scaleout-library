using Fanex.Caching;
using Fanex.SiteDbConnectionManager.Client.Models;

namespace Fanex.SiteDbConnectionManager.Client.Services;

public interface SiteDbClientService
{
    Task<List<SiteDb>> GetActiveSiteDbAsync();

    Task<bool> IsSiteDbActiveAsync(int siteDbGroupId);
}

public class SiteDbClientServiceImp(ICacheService cacheService, SiteDbService siteDbService) : SiteDbClientService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(5);

    public async Task<List<SiteDb>> GetActiveSiteDbAsync()
    {
        const string cacheKey = "settle_tvs_active_sitedbs";
        return await cacheService.GetOrSetAsync(cacheKey,
            async () => await siteDbService.GetActiveSiteDbAsync(),
            new CacheItemOptions().SetAbsoluteExpiration(CacheDuration));
    }

    public async Task<bool> IsSiteDbActiveAsync(int siteDbGroupId)
    {
        var cacheKey = $"settle_tvs_sitedb_status_{siteDbGroupId}";
        return await cacheService.GetOrSetAsync(cacheKey,
            async () => await siteDbService.IsSiteDbActiveAsync(siteDbGroupId),
            new CacheItemOptions().SetAbsoluteExpiration(CacheDuration));
    }
}