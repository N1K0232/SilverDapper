namespace SilverDapper;

public interface IDapperCache
{
    Task RemoveAsync(Guid id);

    Task RemoveAsync(string key);

    Task<TEntity?> GetAsync<TEntity>(Guid id) where TEntity : class;

    Task<IEnumerable<TEntity>?> GetListAsync<TEntity>(string key) where TEntity : class;

    Task SetAsync<TEntity>(Guid id, TEntity entity, TimeSpan absoluteExpiration) where TEntity : class;

    Task SetAsync<TEntity>(string key, IEnumerable<TEntity> entities, TimeSpan absoluteExpiration) where TEntity : class;
}