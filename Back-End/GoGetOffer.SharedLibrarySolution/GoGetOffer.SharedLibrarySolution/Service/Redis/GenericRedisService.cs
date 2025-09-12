using GoGetOffer.SharedLibrarySolution.Interface.Redis;
using GoGetOffer.SharedLibrarySolution.Responses;
using StackExchange.Redis;

namespace GoGetOffer.SharedLibrarySolution.Service.Redis
{
    public class GenericRedisService(IConnectionMultiplexer redis) : IGenericRedisService
    {
        private readonly IDatabase _redis = redis.GetDatabase();

        public async Task<Response<string>> CheckAndCacheRequestAsync(string key, int maxAttempts, TimeSpan expiration)
        {
                var attemptCount = await _redis.StringIncrementAsync(key);

                if (attemptCount == 1)
                    await _redis.KeyExpireAsync(key, expiration);

                if (attemptCount > maxAttempts)
                {
                    var ttl = await _redis.KeyTimeToLiveAsync(key);
                    return Response<string>.Failure("");
                }

                return Response<string>.Success("");
        }
    }
}
