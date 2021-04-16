using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TheGame.Data.Ef;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var logger = provider.GetService<ILogger>();
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
                }
            }
            return host;
        }

        private static void SeedDatabase(TheGameDbContext dbContext, ILogger logger)
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