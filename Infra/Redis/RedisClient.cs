namespace numberPool.Infra.Redis
{
    using StackExchange.Redis;

    public class RedisClient : IRedisClient
    {
        private static readonly ConnectionMultiplexer _CONN;

        static RedisClient()
        {
            _CONN = ConnectionMultiplexer.Connect("localhost");
        }

        public ConnectionMultiplexer Connection => _CONN;

        public IDatabase GetDatabase(int db = -1, object asyncState = null)
        {
            return _CONN.GetDatabase(db, asyncState);
        }
    }
}