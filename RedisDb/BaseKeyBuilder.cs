using StackExchange.Redis;

namespace RedisDb;

public class BaseKeyBuilder : IKeyBuilder
{
    private readonly string _namespace;

    protected BaseKeyBuilder(string ns)
    {
        _namespace = ns;
    }

    public virtual RedisKey Build<T>(params string[] segments)
    {
        var keyString = $"{_namespace}:{typeof(T).Name}:{string.Join('|', segments)}".ToLower();
        return new RedisKey(keyString);
    }
}