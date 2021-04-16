using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using TheGame.Common.Caching;
using TheGame.Domain;
using TheGame.SharedKernel;

namespace TheGame.Infrastructure.Data.Caching
{
    public static class CacheInitializer
    {
        public static IHost PerformInitialCacheLoading(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var logger = provider.GetService<ILogger>();
                var cacheProvider = provider.GetRequiredService<ICacheProvider>();
                var settings = provider.GetRequiredService<TheGameSettings>();

                LoadCache(cacheProvider, settings, logger);
            }

            return host;
        }

        private static void LoadCache(ICacheProvider cacheProvider, TheGameSettings settings, ILogger logger)
        {
            var factory = DbProviderFactories.GetFactory(settings.DataProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = settings.DbConnectionString;
            string sql;

            logger.LogInformation("Loading cache with player data. Please wait...");
            try
            {
                sql = $"SELECT {nameof(Player.Id)} FROM dbo.{nameof(Player)}s";

                connection.Open();

                var players = connection.Query<long>(sql);

                cacheProvider.SetAsync(players, settings.PlayersListCacheKey, null, CancellationToken.None).Wait();
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Unable to load cache with player data!");
            }

            logger.LogInformation("Loading cache with game data. Please wait...");
            try
            {
                sql = $"SELECT {nameof(Game.Id)} FROM dbo.{nameof(Game)}";

                connection.Open();

                var games = connection.Query<long>(sql);

                cacheProvider.SetAsync(games, settings.GamesListCacheKey, null, CancellationToken.None).Wait();
            }
            catch (Exception exc)
            {
                logger?.LogError(exc, "Unable to load cache with game data!");
            }

            if (connection == null)
                return;
            
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
}