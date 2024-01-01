using Microsoft.Extensions.Caching.Memory;

namespace SilverDapper;

internal class DapperCache : IDapperCache
{
    private readonly IMemoryCache cache;

    public DapperCache(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public Task RemoveAsync(Guid id)
    {
        cache.Remove(id);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<TEntity?> GetAsync<TEntity>(Guid id) where TEntity : class
    {
        var entity = cache.Get<TEntity>(id);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<TEntity>?> GetListAsync<TEntity>(string key) where TEntity : class
    {
        var result = cache.TryGetValue<IEnumerable<TEntity>>(key, out var entities);
        if (result)
        {
            return Task.FromResult(entities);
        }

        return Task.FromResult<IEnumerable<TEntity>?>(null);
    }

    public Task SetAsync<TEntity>(Guid id, TEntity entity, TimeSpan absoluteExpiration) where TEntity : class
    {
        cache.Set(id, entity, absoluteExpiration);
        return Task.CompletedTask;
    }

    public Task SetAsync<TEntity>(string key, IEnumerable<TEntity> entities, TimeSpan absoluteExpiration) where TEntity : class
    {
        cache.Set(key, entities, absoluteExpiration);
        return Task.CompletedTask;
    }
}