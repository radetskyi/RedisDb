using StackExchange.Redis;

namespace RedisDb;

internal class RedisDb : IRedisDb
{
    private readonly IKeyBuilder _keyBuilder;
    private readonly IRedisSerializer _redisSerializer;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDb(IConnectionMultiplexer connectionMultiplexer,
        IKeyBuilder keyBuilder, 
        IRedisSerializer redisSerializer)
    {
        _keyBuilder = keyBuilder;
        _redisSerializer = redisSerializer;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IDatabase Db => _connectionMultiplexer.GetDatabase();

    public RedisValue RedisToken => Environment.MachineName;

    public async Task<TEntity> Get<TEntity>(params string[] keySegments)
    {
        var key = _keyBuilder.Build<TEntity>(keySegments);
        var redisValue = await Db.StringGetAsync(key, CommandFlags.PreferReplica);
        if (redisValue.HasValue)
        {
            var entity = _redisSerializer.Deserialize<TEntity>(redisValue);
            return entity;
        }

        return default!;
    }

    public async Task Set<TEntity>(TEntity entity, TimeSpan? expiry, params string[] keySegments)
    {
        var redisKey = _keyBuilder.Build<TEntity>(keySegments);
        var redisValue = _redisSerializer.Serialize(entity);
        if(await Db.LockTakeAsync(redisKey, RedisToken, TimeSpan.FromMicroseconds(300)))
        {
            await  Db.StringSetAsync(redisKey,
                redisValue,
                expiry,
                When.Always,
                CommandFlags.FireAndForget);

            await Db.LockReleaseAsync(redisKey, RedisToken);
        }

        throw new RedLockTakeException();
    }
}
