using StackExchange.Redis;

namespace RedisDb;

internal class RedisDb : IRedisDb
{
    private readonly IKeyBuilder _keyBuilder;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDb(IKeyBuilder keyBuilder, IConnectionMultiplexer connectionMultiplexer)
    {
        _keyBuilder = keyBuilder;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IDatabase Db => _connectionMultiplexer.GetDatabase();

    public async Task<TEntity> Get<TEntity>(params string[] keySegments)
    {
        var key = _keyBuilder.Build<TEntity>(keySegments);
        var redisValue = await Db.StringGetAsync(key, CommandFlags.PreferReplica);
        if (!redisValue.HasValue)
        {
            return default!;
        }
        return default!;
    }

    public void Set<TEntity>(TEntity entity, params string[] keySegments)
    {
        var redisKey = _keyBuilder.Build<TEntity>(keySegments);
        Db.StringSetAsync(redisKey,
            entity!.ToString(),
            TimeSpan.FromMinutes(1), When.Always,
            CommandFlags.FireAndForget);
    }
}