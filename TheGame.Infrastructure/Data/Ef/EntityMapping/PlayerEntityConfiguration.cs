using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    internal class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(x => x.Id)
                   .IsClustered();

            builder.Property(x => x.RegistrationDate)
                    .HasColumnType("datetimeoffset")
                    .IsRequired();

            builder.Property(x => x.Name)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

            builder.HasIndex(x => x.Name)
                   .HasDatabaseName($"IX_UN_{nameof(Player)}_{nameof(Player.Name)}")
                   .IsUnique();

            builder.Property(x => x.Nickname)
                    .HasColumnType("varchar(20)")
                    .IsRequired();

            builder.HasIndex(x => x.Nickname)
                   .HasDatabaseName($"IX_UN_{nameof(Player)}_{nameof(Player.Nickname)}")
                   .IsUnique();

            builder.Property(x => x.ScoreLastUpdateOn);

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<Player> builder)
        {
            var now = DateTimeOffset.UtcNow;
            var players = new List<Player>();
            for (int i = 1; i <= 1000; i++)
                players.Add(new Player { Id = i, Name = $"Player {i}", Nickname = $"player{i}", RegistrationDate = now });

            builder.HasData(players);
        }
    }
}
