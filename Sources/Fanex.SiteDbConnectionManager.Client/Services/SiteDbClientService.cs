using Fanex.Caching;
using Fanex.SiteDbConnectionManager.Client.Models;

namespace Fanex.SiteDbConnectionManager.Client.Services;

public interface SiteDbClientService
{
    Task<List<SiteDb>> GetAllSiteDbAsync();

    Task<List<SiteDb>> GetActiveSiteDbAsync();

    Task<bool> IsSiteDbActiveAsync(int siteDbGroupId);
}

public class SiteDbClientServiceImp(ICacheService cacheService, SiteDbService siteDbService) : SiteDbClientService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(5);

    public async Task<List<SiteDb>> GetAllSiteDbAsync()
    {
        const string cacheKey = "settle_tvs_sitedbs";
        return await cacheService.GetOrSetAsync(cacheKey,
            async () => await siteDbService.GetAllSiteDbAsync(),
            new CacheItemOptions().SetAbsoluteExpiration(CacheDuration));
    }

    public async Task<List<SiteDb>> GetActiveSiteDbAsync()
    {
        var allSiteDBs = await GetAllSiteDbAsync();
        return allSiteDBs.Where(x => x.Status).ToList();
    }

    public async Task<bool> IsSiteDbActiveAsync(int siteDbGroupId)
    {
        var cacheKey = $"settle_tvs_sitedb_status_{siteDbGroupId}";
        return await cacheService.GetOrSetAsync(cacheKey,
            async () => await siteDbService.IsSiteDbActiveAsync(siteDbGroupId),
            new CacheItemOptions().SetAbsoluteExpiration(CacheDuration));
    }
}