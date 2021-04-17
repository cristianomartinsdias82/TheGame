using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Commands.SaveMatchData;
using TheGame.Domain;
using TheGame.SharedKernel;
using Z.Dapper.Plus;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Infrastructure.Commands.SaveMatchData.Repository
{
    internal class TheGameCommandsRepository : ITheGameCommandsRepository
    {
        private readonly TheGameSettings _settings;

        public TheGameCommandsRepository(
            TheGameSettings settings)
        {
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task BulkInsertGameMatchesPlayersAsync(IEnumerable<GameMatchesPlayers> matchData, CancellationToken cancellationToken)
        {
            var factory = DbProviderFactories.GetFactory(_settings.DataProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = _settings.DbConnectionString;

            try
            {
                await connection.OpenAsync(cancellationToken);

                connection.BulkInsert(matchData);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task BulkUpdatePlayersAsync(IEnumerable<Player> players, CancellationToken cancellationToken)
        {
            var factory = DbProviderFactories.GetFactory(_settings.DataProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = _settings.DbConnectionString;

            try
            {
                await connection.OpenAsync(cancellationToken);

                var context = new DapperPlusContext
                {
                    Connection = connection
                };
                context.Entity<Player>().Table($"{_settings.ObjectsSchema}.{nameof(Player)}s");

                connection.BulkUpdate(context, players);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}