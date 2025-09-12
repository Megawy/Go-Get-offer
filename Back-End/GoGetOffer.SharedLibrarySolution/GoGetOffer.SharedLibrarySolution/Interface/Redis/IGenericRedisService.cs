using GoGetOffer.SharedLibrarySolution.Responses;

namespace GoGetOffer.SharedLibrarySolution.Interface.Redis
{
    public interface IGenericRedisService
    {
        Task<Response<string>> CheckAndCacheRequestAsync(string key, int maxRequests, TimeSpan window);
    }
}
