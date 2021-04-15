using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Commands.Repositories;
using TheGame.Data.Ef;
using TheGame.Domain;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Infrastructure.Commands.SaveMatchData.Repository
{
    internal class TheGameCommandsRepository : ITheGameCommandsRepository
    {
        private readonly TheGameDbContext _dbContext;

        public TheGameCommandsRepository(TheGameDbContext dbContext)
        {
            _dbContext = dbContext ?? throw ArgNullEx(nameof(dbContext));
        }

        public async Task BulkInsertGameMatchesPlayersAsync(IEnumerable<GameMatchesPlayers> matchData, CancellationToken cancellationToken)
        {
            await _dbContext.BulkInsertAsync(matchData, cancellationToken);
        }

        public async Task BulkUpdatePlayersAsync(IEnumerable<Player> players, CancellationToken cancellationToken)
        {
            await _dbContext.BulkUpdateAsync(players, cancellationToken);
        }
    }
}