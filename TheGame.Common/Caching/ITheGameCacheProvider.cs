using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Dto;

namespace TheGame.Common.Caching
{
    public interface ITheGameCacheProvider : ICacheProvider
    {
        Task StoreGameMatchesAsync(IEnumerable<CacheItem<GameMatchDataDto>> gameMatches, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);
        Task<IEnumerable<CacheItem<GameMatchDataDto>>> GetGameMatchesAsync(CancellationToken cancellationToken);
    }
}