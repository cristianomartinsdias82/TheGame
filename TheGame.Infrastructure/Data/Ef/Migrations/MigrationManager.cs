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
                var logger = provider.GetService<ILogger<IHost>>();
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
                        logger?.LogError(exc, "Unable seed the database!");

                        throw;
                    }
                }
            }
            return host;
        }

        private static void SeedDatabase(TheGameDbContext dbContext, ILogger<IHost> logger)
        {
            bool saveChanges = false;

            if (!dbContext.Players.Any())
            {
                logger.LogInformation("Seeding database with players data. Please wait...");

                var now = DateTimeOffset.UtcNow;
                var players = new List<Player>();
                for (int i = 1; i <= 1000; i++)
                    players.Add(new Player { Name = $"Player {i}'s name", Nickname = $"player{i}", RegistrationDate = now, GameMatchesPlayers = new List<GameMatchesPlayers>() });

                dbContext.Players.AddRange(players);
                saveChanges = true;
            }

            if (!dbContext.GameMatches.Any())
            {
                logger.LogInformation("Seeding database with game matches data. Please wait...");

                var now = DateTimeOffset.UtcNow;
                var gameMatches = new List<GameMatch>();
                for (int i = 1; i <= 100; i++)
                    gameMatches.Add(new GameMatch { Title = $"Match {i}", RegistrationDate = now, GameMatchesPlayers = new List<GameMatchesPlayers>() });

                dbContext.GameMatches.AddRange(gameMatches);
                saveChanges = true;
            }

            if (saveChanges)
            {
                dbContext.SaveChanges();
                logger.LogInformation("Database seeding successful!");
            }
        }
    }
}
