using Fanex.SiteDbConnectionManager.Client.Models;
using Fanex.SiteDbConnectionManager.Client.Resilience;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Fanex.SiteDbConnectionManager.Client.Services;

public interface SiteDbService
{
    Task<List<SiteDb>> GetAllSiteDbAsync();

    Task<List<SiteDb>> GetActiveSiteDbAsync();

    Task<bool> IsSiteDbActiveAsync(int siteDbGroupId);

    Task<SiteDb> AddSiteDbAsync(UpdateSiteDbStatus request);

    Task<SiteDb> UpdateSiteDbStatusAsync(UpdateSiteDbStatus request);

    Task<bool> RemoveSiteDbAsync(int siteDbGroupId);
}

/// <summary>
/// Redis-based implementation of SiteDbService
/// </summary>
public sealed class SiteDbServiceImp(IConnectionMultiplexer redis) : SiteDbService
{
    private const string RedisKeyPrefix = "sitedb:info:";

    private static readonly ConcurrentDictionary<int, SiteDb> LocalCache = new();

    private readonly RedisCircuitBreaker circuitBreaker = new(redis);

    public async Task<List<SiteDb>> GetAllSiteDbAsync()
    {
        var sites = await circuitBreaker.ExecuteAsync(async db =>
        {
            var server = redis.GetServer(redis.GetEndPoints()[0]);
            var keys = server.Keys(pattern: $"{RedisKeyPrefix}*").ToArray();

            if (keys.Length == 0 && !LocalCache.IsEmpty)
            {
                return LocalCache.Values.ToList();
            }

            var values = await db.StringGetAsync(keys);
            var siteDBs = new List<SiteDb>(keys.Length);

            foreach (var value in values.Where(x => x.HasValue))
            {
                var siteDb = JsonSerializer.Deserialize<SiteDb>(value!, Helper.JsonOptions);
                if (siteDb == null)
                {
                    continue;
                }

                siteDBs.Add(siteDb);
                LocalCache.AddOrUpdate(siteDb.SiteDbGroupId, siteDb, (_, _) => siteDb);
            }

            return siteDBs;
        }, LocalCache.Values.ToList());

        return sites.OrderBy(x => x.SiteDbGroupId).ToList();
    }

    public async Task<List<SiteDb>> GetActiveSiteDbAsync()
    {
        var allSiteDBs = await GetAllSiteDbAsync();
        return allSiteDBs.Where(x => x.Status).ToList();
    }

    public async Task<bool> IsSiteDbActiveAsync(int siteDbGroupId)
    {
        return await circuitBreaker.ExecuteAsync(async db =>
        {
            var key = GetRedisKey(siteDbGroupId);
            var value = await db.StringGetAsync(key);

            if (!value.HasValue)
            {
                return LocalCache.TryGetValue(siteDbGroupId, out var cachedSiteDb) && cachedSiteDb.Status;
            }

            var siteDb = JsonSerializer.Deserialize<SiteDb>(value!, Helper.JsonOptions);
            if (siteDb != null)
            {
                LocalCache.AddOrUpdate(siteDb.SiteDbGroupId, siteDb, (_, _) => siteDb);
            }

            return siteDb?.Status ?? false;
        }, LocalCache.TryGetValue(siteDbGroupId, out var cached) && cached.Status);
    }

    public async Task<SiteDb> AddSiteDbAsync(UpdateSiteDbStatus request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var db = redis.GetDatabase();
        var key = GetRedisKey(request.SiteDbGroupId);

        if (await db.KeyExistsAsync(key))
        {
            throw new InvalidOperationException($"SiteDB with ID {request.SiteDbGroupId} already exists");
        }

        var siteDb = new SiteDb
        {
            SiteDbGroupId = request.SiteDbGroupId,
            SiteName = request.SiteName,
            Status = request.Status
        };

        var serializedValue = JsonSerializer.Serialize(siteDb, Helper.JsonOptions);
        await db.StringSetAsync(key, serializedValue, when: When.NotExists);

        return siteDb;
    }

    public async Task<SiteDb> UpdateSiteDbStatusAsync(UpdateSiteDbStatus request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var db = redis.GetDatabase();
        var key = GetRedisKey(request.SiteDbGroupId);

        var siteDb = new SiteDb
        {
            SiteDbGroupId = request.SiteDbGroupId,
            SiteName = request.SiteName,
            Status = request.Status
        };

        var serializedValue = JsonSerializer.Serialize(siteDb, Helper.JsonOptions);
        await db.StringSetAsync(key, serializedValue);

        return siteDb;
    }

    public async Task<bool> RemoveSiteDbAsync(int siteDbGroupId)
    {
        var db = redis.GetDatabase();
        var key = GetRedisKey(siteDbGroupId);

        var result = await db.KeyDeleteAsync(key);
        return result;
    }

    private static string GetRedisKey(int siteDbGroupId) => $"{RedisKeyPrefix}{siteDbGroupId}";
}