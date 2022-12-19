using StackExchange.Redis;

namespace RedisDb;

public interface IKeyBuilder
{
    RedisKey Build<T>(params string[] segments);
}
