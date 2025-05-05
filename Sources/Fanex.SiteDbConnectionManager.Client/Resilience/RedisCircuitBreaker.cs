using StackExchange.Redis;

namespace Fanex.SiteDbConnectionManager.Client.Resilience;

public class RedisCircuitBreaker(IConnectionMultiplexer redis)
{
    private readonly SemaphoreSlim @lock = new(1, 1);
    private bool isOpen;
    private DateTime lastFailure;

    private const int BreakDurationSeconds = 30;

    public async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<T>> operation, T fallbackValue)
    {
        if (isOpen)
        {
            if (DateTime.UtcNow - lastFailure > TimeSpan.FromSeconds(BreakDurationSeconds))
            {
                await @lock.WaitAsync();
                try
                {
                    isOpen = false;
                }
                finally
                {
                    @lock.Release();
                }
            }
            else
            {
                return fallbackValue;
            }
        }

        try
        {
            var db = redis.GetDatabase();
            return await operation(db);
        }
        catch (RedisConnectionException)
        {
            await @lock.WaitAsync();
            try
            {
                isOpen = true;
                lastFailure = DateTime.UtcNow;
            }
            finally
            {
                @lock.Release();
            }
            return fallbackValue;
        }
    }
}