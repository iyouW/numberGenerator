namespace numberPool.App.Services.NumberPool
{
    using System.Threading.Tasks;
    using numberPool.Infra.Redis;
    public class NumberPoolService : INumberPoolService
    {
        public const string REDIS_KEY = "31_TRTC_ROOM_TEST_2";
        public const long MAX_NUMBER_ = 4294967294;

        public const long MAX_NUMBER = 8*100;
        private readonly IRedisClient _redis;

        public NumberPoolService(IRedisClient redis)
        {
            _redis = redis;
        }

        public async Task<long> RentAsync()
        {
            
            var db = _redis.GetDatabase();
            var res = await db.StringBitPositionAsync(REDIS_KEY, false);
            await db.StringSetBitAsync(REDIS_KEY, res, true);
            return res;
        }

        public async Task ReturnAsync(long number)
        {
            var db = _redis.GetDatabase();
            await db.StringSetBitAsync(REDIS_KEY, number, false);
        }
    }
}