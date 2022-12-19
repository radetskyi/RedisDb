using StackExchange.Redis;

namespace RedisDb;

internal interface IRedisSerializer
{
    RedisValue Serialize<TEntity>(TEntity entity);

    TEntity Deserialize<TEntity>(RedisValue redisValue);
}