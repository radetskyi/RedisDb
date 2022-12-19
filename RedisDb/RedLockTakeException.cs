namespace RedisDb;

internal class RedLockTakeException : Exception
{
    public RedLockTakeException(string? message = null) : base(message)
    {
    }
}
