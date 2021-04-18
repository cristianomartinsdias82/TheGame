using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TheGame.Data.Ef;
using TheGame.Domain;
using TheGame.SharedKernel;

namespace TheGame.Infrastructure.Data.Ef.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var logger = provider.GetService<ILogger<IHost>>();
                var settings = provider.GetService<TheGameSettings>();
                using (var dbContext = provider.GetRequiredService<TheGameDbContext>())
                {
                    try
                    {
                        dbContext.Database.Migrate();
                    }
                    catch (Exception exc)
                    {
                        logger?.LogError(exc, "Unable to migrate database!");

                        throw;
                    }

                    try
                    {
                        SeedDatabase(dbContext, logger);
                    }
                    catch (Exception exc)
                    {
                        logger?.LogError(exc, "Unable to seed the database!");

                        throw;
                    }

                    const string viewName = "V_Leaderboards";
                    try
                    {
                        CreateLeaderboardsView(dbContext, viewName, settings.ObjectsSchema, $"{viewName}.sql", logger);
                    }
                    catch (Exception exc)
                    {
                        logger?.LogError(exc, "Unable to create Leaderboards view object!");

                        throw;
                    }
                }
            }
            return host;
        }

        private static void CreateLeaderboardsView(TheGameDbContext dbContext, string viewName, string schemaName, string resourceFileName, ILogger<IHost> logger)
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));
            var resource = assembly.GetManifestResourceStream($"{assemblyName}.Resources.{resourceFileName}");
            var resourceStream = new StreamReader(resource);

            var sql = resourceStream.ReadToEnd();
            resourceStream.Close();

            dbContext.Database.ExecuteSqlRaw($"IF OBJECT_ID('{schemaName}.{viewName}') IS NOT NULL BEGIN DROP VIEW {schemaName}.{viewName} END");
            dbContext.Database.ExecuteSqlRaw($"CREATE VIEW {viewName} AS {sql}");
        }

        private static void SeedDatabase(TheGameDbContext dbContext, ILogger<IHost> logger)
        {
            bool hasPendingData = false;

            if (!dbContext.Players.Any())
            {
                logger.LogInformation("Seeding database with player data. Please wait...");

                var now = DateTimeOffset.UtcNow;
                var players = new List<Player>();
                for (int i = 1; i <= 1000; i++)
                    players.Add(new Player { Name = $"Player {i}", Nickname = $"player{i}", RegistrationDate = now, GameMatchesPlayers = new List<GameMatchesPlayers>() });

                dbContext.Players.AddRange(players);
                hasPendingData = true;
            }

            if (!dbContext.Games.Any())
            {
                logger.LogInformation("Seeding database with game data. Please wait...");

                var now = DateTimeOffset.UtcNow;
                var games = new List<Game>();
                for (int i = 1; i <= 20; i++)
                    games.Add(new Game { Title = $"Game {i}", RegistrationDate = now });

                dbContext.Games.AddRange(games);
                hasPendingData = true;
            }

            if (hasPendingData)
            {
                dbContext.SaveChanges();
                logger.LogInformation("Database seed successful!");
            }
        }
    }
}