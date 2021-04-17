using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Infrastructure.Data.Caching
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        protected readonly IMemoryCache _memoryCache;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw ArgNullEx(nameof(memoryCache));
        }

        public virtual async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            T data;
            _memoryCache.TryGetValue(key, out data);

            return await Task.FromResult(data);
        }

        public virtual async Task SetAsync<T>(T data, string key, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        {
            await SafeUseCache(
                key,
                (key) =>
                {
                    if (absoluteExpiration.HasValue)
                        _memoryCache.Set(key, data, absoluteExpiration.Value);
                    else
                        _memoryCache.Set(key, data);
                },
                cancellationToken);
        }

        protected virtual async Task SafeUseCache(string key, Action<string> action, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                action(key);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
