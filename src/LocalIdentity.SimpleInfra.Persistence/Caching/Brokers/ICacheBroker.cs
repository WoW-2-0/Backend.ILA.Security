using LocalIdentity.SimpleInfra.Domain.Common.Caching;

namespace LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;

public interface ICacheBroker
{
    ValueTask<T?> GetAsync<T>(string key);

    ValueTask<bool> TryGetAsync<T>(string key, out T? value);

    ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = default);

    ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default);

    ValueTask DeleteAsync(string key);
}