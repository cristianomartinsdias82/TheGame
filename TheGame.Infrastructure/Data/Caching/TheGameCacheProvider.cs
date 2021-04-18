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

        public TheGameCacheProvider(IMemoryCache memoryCache, TheGameSettings settings) : base(memoryCache)
        {
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task StoreGameMatchesAsync(IEnumerable<CacheItem<GameMatchDataDto>> gameMatches, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        => await SetAsync(gameMatches, _settings.GameMatchesDataCacheKey, absoluteExpiration, cancellationToken);

        public async Task<IEnumerable<CacheItem<GameMatchDataDto>>> GetGameMatchesAsync(CancellationToken cancellationToken)
        => await GetAsync<IEnumerable<CacheItem<GameMatchDataDto>>>(_settings.GameMatchesDataCacheKey, cancellationToken);

        public async Task StoreGamesListAsync(IEnumerable<long> gamesList, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        => await SetAsync(gamesList, _settings.GamesListCacheKey, absoluteExpiration, cancellationToken);

        public async Task<IEnumerable<long>> GetGamesListAsync(CancellationToken cancellationToken)
        => await GetAsync<IEnumerable<long>>(_settings.GamesListCacheKey, cancellationToken);

        public async Task StorePlayersListAsync(IEnumerable<long> playersList, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        => await SetAsync(playersList, _settings.PlayersListCacheKey, absoluteExpiration, cancellationToken);

        public async Task<IEnumerable<long>> GetPlayersListAsync(CancellationToken cancellationToken)
        => await GetAsync<IEnumerable<long>>(_settings.PlayersListCacheKey, cancellationToken);

        public async Task<IEnumerable<PlayerBalanceDto>> GetLeaderboardsAsync(CancellationToken cancellationToken)
        => await GetAsync<IEnumerable<PlayerBalanceDto>>(_settings.LeaderboardsCacheKey, cancellationToken);

        public async Task StoreLeaderboardsAsync(IEnumerable<PlayerBalanceDto> leaderboards, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken)
        => await SetAsync(leaderboards, _settings.LeaderboardsCacheKey, absoluteExpiration, cancellationToken);
    }
}