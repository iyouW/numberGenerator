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
        public const string REDIS_KEY = "31_TRTC_ROOM_TEST_2";
        private const string RESOURCE_KEY = "31_TRTC_ROOM_MUBER";

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
            if(!await db.KeyExistsAsync(REDIS_KEY))
            {
                await db.StringSetBitAsync(REDIS_KEY, MAX_NUMBER, false);
            }
            using(var redLock = await _redLockFactory.CreateLockAsync(RESOURCE_KEY, TimeSpan.FromSeconds(10)))
            {
                if(redLock.IsAcquired)
                {
                    var res = await db.StringBitPositionAsync(REDIS_KEY, false);
                    await db.StringSetBitAsync(REDIS_KEY, res, true);
                    return res;
                }
                else
                {
                    return -1;
                }
            }
        }

        public async Task ReturnAsync(long number)
        {
            var db = _redis.GetDatabase();
            await db.StringSetBitAsync(REDIS_KEY, number, false);
        }
    }
}