using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Dto;
using TheGame.Domain;

namespace TheGame.Queries.GetLeaderboards.Repositories
{
    public interface ITheGameQueriesRepository
    {
        Task<IEnumerable<PlayerBalanceDto>> FetchLeaderboardsAsync(int playersMaxQuantity, CancellationToken cancellationToken);

        Task<IEnumerable<Player>> FetchPlayersByIdsAsync(IEnumerable<long> playersIds, CancellationToken cancellationToken);
    }
}
