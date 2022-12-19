namespace RedisDb;

public interface IKeyBuilder
{
    string Build(params string[] segments);
}