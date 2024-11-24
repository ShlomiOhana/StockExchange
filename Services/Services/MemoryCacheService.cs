using Microsoft.Extensions.Caching.Memory;
using Services.Models;

namespace AppServices.Services
{
    public class MemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string cacheKey = "CachedStocks";
        private readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(15);  // Cache data for 15 minutes

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public List<Stock> GetStocks()
        {
            List<Stock> stocks = new List<Stock>();
            if (_memoryCache.TryGetValue(cacheKey, out List<Stock> cachedStocks))
            {
                if (cachedStocks != null)
                {
                    stocks = cachedStocks;
                }
            }

            return stocks;
        }

        public void SetStocks(List<Stock> stocksList)
        {
            _memoryCache.Set(cacheKey, stocksList, cacheDuration);
        }
    }
}
