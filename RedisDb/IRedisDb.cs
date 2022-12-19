using StackExchange.Redis;

namespace RedisDb;

public interface IRedisDb
{ 
    IDatabase Db { get; }

    RedisValue RedisToken { get; }

    Task<TEntity> Get<TEntity>(params string[] keySegments);

    Task Set<TEntity>(TEntity entity, TimeSpan? expiry, params string[] keySegments);
}
