using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Infrastructure.Data.Caching
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw ArgNullEx(nameof(memoryCache));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            T data;
            _memoryCache.TryGetValue(key, out data);

            return await Task.FromResult(data);
        }

        public async Task SetAsync<T>(T data, string key, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        {
            if (absoluteExpiration.HasValue)
                _memoryCache.Set(key, data, absoluteExpiration.Value);
            else
                _memoryCache.Set(key, data);

            await Task.CompletedTask;
        }

        public async Task ClearAsync(string key, CancellationToken cancellationToken)
        {
            _memoryCache.Remove(key);

            await Task.CompletedTask;
        }
    }
}
