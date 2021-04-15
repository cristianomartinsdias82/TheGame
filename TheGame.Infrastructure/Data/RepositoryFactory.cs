using TheGame.Commands.Repositories;
using TheGame.Data.Ef;
using TheGame.Infrastructure.Commands.SaveMatchData.Repository;
using TheGame.Infrastructure.Queries.GetLeaderboards.Repository;
using TheGame.Queries.GetLeaderboards.Repositories;
using TheGame.SharedKernel;

namespace TheGame.Infrastructure.Data
{
    internal static class RepositoryFactory
    {
        public static ITheGameQueriesRepository GetQueriesRepository(TheGameSettings settings)
        => new TheGameQueriesRepository(settings);

        public static ITheGameCommandsRepository GetCommandsRepository(TheGameDbContext dbContext)
        => new TheGameCommandsRepository(dbContext);
    }
}
