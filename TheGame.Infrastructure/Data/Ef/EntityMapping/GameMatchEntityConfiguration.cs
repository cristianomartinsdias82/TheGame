using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    internal class GameMatchEntityConfiguration : IEntityTypeConfiguration<GameMatch>
    {
        public void Configure(EntityTypeBuilder<GameMatch> builder)
        {
            builder.HasKey(x => x.Id)
                   .IsClustered();

            builder.Property(x => x.RegistrationDate)
                    .HasColumnType("datetimeoffset")
                    .IsRequired();

            builder.Property(x => x.Title)
                    .HasColumnType("varchar(40)")
                    .IsRequired();

            builder.HasIndex(x => x.Title)
                   .HasDatabaseName($"IX_UN_{nameof(GameMatch)}_{nameof(GameMatch.Title)}")
                   .IsUnique();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<GameMatch> builder)
        {
            var now = DateTimeOffset.UtcNow;
            var gameMatches = new List<GameMatch>();
            for (int i = 1; i <= 100; i++)
                gameMatches.Add(new GameMatch { Id = i, Title = $"Match {i}", RegistrationDate = now, GameMatchesPlayers = new List<GameMatchesPlayers>() });

            builder.HasData(gameMatches);
        }
    }
}
