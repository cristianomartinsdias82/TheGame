using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Domain;

namespace TheGame.Commands.SaveMatchData
{
    public interface ITheGameCommandsRepository
    {
        Task BulkInsertGameMatchesPlayersAsync(IEnumerable<GameMatchesPlayers> matchData, CancellationToken cancellationToken);
        Task BulkUpdatePlayersAsync(IEnumerable<Player> players, CancellationToken cancellationToken);
    }
}