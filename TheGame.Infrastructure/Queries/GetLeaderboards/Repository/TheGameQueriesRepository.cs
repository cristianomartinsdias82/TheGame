using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Dto;
using TheGame.Domain;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Infrastructure.Queries.GetLeaderboards.Repository
{
    internal class TheGameQueriesRepository : ITheGameQueriesRepository
    {
        private readonly TheGameSettings _settings;

        public TheGameQueriesRepository(TheGameSettings settings)
        {
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task<IEnumerable<PlayerBalanceDto>> FetchLeaderboardsAsync(int playersMaxQuantity, CancellationToken cancellationToken)
        {
            var factory = DbProviderFactories.GetFactory(_settings.DataProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = _settings.DbConnectionString;

            var parameters = new DynamicParameters();
            parameters.Add("top", playersMaxQuantity, DbType.Int32, ParameterDirection.Input);

            var sql = $"SELECT TOP({playersMaxQuantity}) {nameof(PlayerBalanceDto.PlayerId)}, {nameof(PlayerBalanceDto.Balance)}, {nameof(PlayerBalanceDto.ScoreLastUpdateOn)} FROM dbo.V_Leaderboards ORDER BY Balance DESC";

            IEnumerable<PlayerBalanceDto> leaderboards;
            try
            {
                await connection.OpenAsync(cancellationToken);

                leaderboards = await connection.QueryAsync<PlayerBalanceDto>(sql);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }

            return leaderboards ?? new PlayerBalanceDto[0];
        }

        public async Task<IEnumerable<Player>> FetchPlayersByIdsAsync(IEnumerable<long> playersIds, CancellationToken cancellationToken)
        {
            var factory = DbProviderFactories.GetFactory(_settings.DataProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = _settings.DbConnectionString;

            var commandDef = new CommandDefinition(
                $"SELECT {nameof(Player.Id)}, {nameof(Player.Name)}, {nameof(Player.Nickname)}, {nameof(Player.RegistrationDate)}, {nameof(Player.ScoreLastUpdateOn)} FROM {_settings.ObjectsSchema}.Players WHERE {nameof(Player.Id)} IN ({string.Join(",", playersIds.Select(x => x.ToString()))})",
                cancellationToken: cancellationToken);

            IEnumerable<Player> players;
            try
            {
                await connection.OpenAsync(cancellationToken);

                players = await connection.QueryAsync<Player>(commandDef);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }

            return players ?? new Player[0];
        }
    }
}
