using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Infrastructure.Data.Caching
{
    public sealed class TheGameCacheProvider : InMemoryCacheProvider, ITheGameCacheProvider
    {
        private readonly TheGameSettings _settings;

        public TheGameCacheProvider(
            IMemoryCache memoryCache,
            TheGameSettings settings) : base(memoryCache)
        {
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task StoreGameMatchesAsync(IEnumerable<CacheItem<GameMatchDataDto>> gameMatches, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        => await SetAsync(gameMatches, _settings.GameMatchesDataCacheKey, absoluteExpiration, cancellationToken);

        public async Task<IEnumerable<CacheItem<GameMatchDataDto>>> GetGameMatchesAsync(CancellationToken cancellationToken)
        => await GetAsync<IEnumerable<CacheItem<GameMatchDataDto>>>(_settings.GameMatchesDataCacheKey, cancellationToken);
    }
}
