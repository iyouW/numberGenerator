namespace numberPool.Infra.Redis
{
    using StackExchange.Redis;

    public interface IRedisClient
    {
        ConnectionMultiplexer Connection{get;}
        IDatabase GetDatabase(int db = -1, object asyncState = null);

    }
}