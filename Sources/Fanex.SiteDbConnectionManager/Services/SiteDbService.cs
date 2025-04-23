using Fanex.SiteDbConnectionManager.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Fanex.SiteDbConnectionManager.Services;

public interface SiteDbService
{
    Task<List<SiteDb>> GetAllSiteDbAsync();

    Task<List<SiteDb>> GetActiveSiteDbAsync();

    Task<bool> IsSiteDbActiveAsync(int siteDbGroupId);

    Task<SiteDb> AddSiteDbAsync(UpdateSiteDbStatus request);

    Task<SiteDb> UpdateSiteDbStatusAsync(UpdateSiteDbStatus request);
}

public class SiteDbServiceImp(IConnectionMultiplexer redis) : SiteDbService
{
    private const string REDIS_KEY_PREFIX = "sitedb:info:";

    public async Task<List<SiteDb>> GetAllSiteDbAsync()
    {
        var db = redis.GetDatabase();
        var server = redis.GetServer(redis.GetEndPoints()[0]);
        var keys = server.Keys(pattern: $"{REDIS_KEY_PREFIX}*").ToArray();
        var siteDBs = new List<SiteDb>();

        foreach (var key in keys)
        {
            var value = await db.StringGetAsync(key);
            if (value.HasValue)
            {
                siteDBs.Add(JsonSerializer.Deserialize<SiteDb>(value));
            }
        }

        return siteDBs;
    }

    public async Task<List<SiteDb>> GetActiveSiteDbAsync()
    {
        var allSiteDBs = await GetAllSiteDbAsync();
        return allSiteDBs.Where(x => x.Status).ToList();
    }

    public async Task<bool> IsSiteDbActiveAsync(int siteDbGroupId)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync($"{REDIS_KEY_PREFIX}{siteDbGroupId}");

        if (!value.HasValue)
        {
            return false;
        }

        var siteDb = JsonSerializer.Deserialize<SiteDb>(value);
        return siteDb.Status;
    }

    public async Task<SiteDb> AddSiteDbAsync(UpdateSiteDbStatus request)
    {
        var db = redis.GetDatabase();
        var key = $"{REDIS_KEY_PREFIX}{request.SiteDbGroupId}";

        if (await db.KeyExistsAsync(key))
        {
            throw new InvalidOperationException($"SiteDB with ID {request.SiteDbGroupId} already exists");
        }

        var siteDB = new SiteDb
        {
            SiteDbGroupId = request.SiteDbGroupId,
            SiteName = request.SiteName,
            Status = request.Status,
        };

        await db.StringSetAsync(key, JsonSerializer.Serialize(siteDB), when: When.NotExists);

        return siteDB;
    }

    public async Task<SiteDb> UpdateSiteDbStatusAsync(UpdateSiteDbStatus request)
    {
        var db = redis.GetDatabase();
        var key = $"{REDIS_KEY_PREFIX}{request.SiteDbGroupId}";

        var info = new SiteDb
        {
            SiteDbGroupId = request.SiteDbGroupId,
            SiteName = request.SiteName,
            Status = request.Status
        };

        await db.StringSetAsync(key, JsonSerializer.Serialize(info));
        return info;
    }
}