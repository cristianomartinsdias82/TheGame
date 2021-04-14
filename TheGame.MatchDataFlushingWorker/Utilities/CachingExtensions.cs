using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.SharedKernel;

namespace TheGame.MatchDataFlushingWorker.Utilities
{
    internal static class CachingExtensions
    {
        public async static Task<IEnumerable<MatchDataDto>> GetMatchDataAsync(this ICacheProvider cacheProvider, TheGameSettings settings, CancellationToken cancellationToken)
            => await cacheProvider.GetAsync<IEnumerable<MatchDataDto>>(settings.GameMatchesDataCacheKey, cancellationToken);
    }
}
