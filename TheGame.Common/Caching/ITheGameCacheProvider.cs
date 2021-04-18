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

        Task StoreGamesListAsync(IEnumerable<long> gamesList, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);

        Task<IEnumerable<long>> GetGamesListAsync(CancellationToken cancellationToken);

        Task StorePlayersListAsync(IEnumerable<long> playersList, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);

        Task<IEnumerable<long>> GetPlayersListAsync(CancellationToken cancellationToken);

        Task StoreLeaderboardsAsync(IEnumerable<PlayerBalanceDto> leaderboards, DateTimeOffset? absoluteExpiration, CancellationToken cancellationToken);

        Task<IEnumerable<PlayerBalanceDto>> GetLeaderboardsAsync(CancellationToken cancellationToken);
    }
}