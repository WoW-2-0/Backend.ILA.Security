﻿using System.Text;
using Force.DeepCloner;
using LocalIdentity.SimpleInfra.Domain.Common.Caching;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Caching.Brokers;

/// <summary>
/// /// Represents a cache broker implementation for interacting with a Redis-based distributed cache.
/// </summary>
/// <param name="cacheSettings"></param>
/// <param name="distributedCache"></param>
public class RedisDistributedCacheBroker(IOptions<CacheSettings> cacheSettings, IDistributedCache distributedCache) : ICacheBroker
{
    private readonly DistributedCacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheSettings.Value.AbsoluteExpirationInSeconds),
        SlidingExpiration = TimeSpan.FromSeconds(cacheSettings.Value.SlidingExpirationInSeconds)
    };

    public async ValueTask<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await distributedCache.GetAsync(key, cancellationToken);
        return value is not null ? JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value)) : default;
    }

    public ValueTask<bool> TryGetAsync<T>(string key, out T? value, CancellationToken cancellationToken = default)
    {
        var foundEntry = distributedCache.GetString(key);

        if (foundEntry is not null)
        {
            value = JsonConvert.DeserializeObject<T>(foundEntry);
            return ValueTask.FromResult(true);
        }

        value = default;
        return ValueTask.FromResult(false);
    }

    public async ValueTask<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> valueFactory,
        CacheEntryOptions? entryOptions = default,
        CancellationToken cancellationToken = default
    )
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedValue is not null) return JsonConvert.DeserializeObject<T>(cachedValue);

        var value = await valueFactory();
        await SetAsync(key, await valueFactory(), entryOptions, cancellationToken);

        return value;
    }

    public async ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), GetCacheEntryOptions(entryOptions), cancellationToken);
    }

    public ValueTask DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        distributedCache.Remove(key);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Gets the cache entry options based on given entry options or default options.
    /// </summary>
    /// <param name="entryOptions">Given cache entry options.</param>
    /// <returns>The distributed cache entry options.</returns>
    private DistributedCacheEntryOptions GetCacheEntryOptions(CacheEntryOptions? entryOptions)
    {
        if (entryOptions == default || (!entryOptions.AbsoluteExpirationRelativeToNow.HasValue && !entryOptions.SlidingExpiration.HasValue))
            return _entryOptions;

        var currentEntryOptions = _entryOptions.DeepClone();

        currentEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow.HasValue
            ? currentEntryOptions.AbsoluteExpirationRelativeToNow
            : null;

        currentEntryOptions.SlidingExpiration = entryOptions.SlidingExpiration.HasValue ? currentEntryOptions.SlidingExpiration : null;

        return currentEntryOptions;
    }
}