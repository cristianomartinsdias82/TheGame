using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    internal class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
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
                   .HasDatabaseName($"IX_UN_{nameof(Game)}_{nameof(Game.Title)}")
                   .IsUnique();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Game> builder)
        {
            var now = DateTimeOffset.UtcNow;
            var gameMatches = new List<Game>();
            for (int i = 1; i <= 20; i++)
                gameMatches.Add(new Game { Id = i, Title = $"Game {i}", RegistrationDate = now });

            builder.HasData(gameMatches);
        }
    }
}
