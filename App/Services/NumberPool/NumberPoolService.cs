namespace numberPool.App.Services.NumberPool
{
    using System;
    using System.Threading.Tasks;
    using numberPool.Infra.Redis;
    using RedLockNet;
    using RedLockNet.SERedis;
    using RedLockNet.SERedis.Configuration;
    using System.Collections.Generic;
    
    public class NumberPoolService : INumberPoolService
    {
        public const string REDIS_KEY = "31_TRTC_ROOM_TEST_16";

        //trtc max room number
        public const long MAX_NUMBER = 4294967294;

        private readonly IRedisClient _redis;

        private readonly IDistributedLockFactory _redLockFactory;

        public NumberPoolService(IRedisClient redis)
        {
            _redis = redis;
            _redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> {_redis.Connection});
        }

        public async Task<long> RentAsync()
        {
            var db = _redis.GetDatabase();
            var randon = new Random(1);
            while(true)
            {
                if(await db.HashLengthAsync(REDIS_KEY) >= MAX_NUMBER)
                {
                    return -1;
                }
                var number = Math.Round(randon.NextDouble() * 10_000_000_000) % MAX_NUMBER;
                if(await db.HashSetAsync(REDIS_KEY, number, 1 , StackExchange.Redis.When.NotExists))
                {
                    return (long)number;
                }
            }
        }

        public async Task ReturnAsync(long number)
        {
            var db = _redis.GetDatabase();
            await db.HashDeleteAsync(REDIS_KEY, number);
        }
    }
}