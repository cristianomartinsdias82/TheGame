using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    internal class GameMatchesPlayersEntityConfiguration : IEntityTypeConfiguration<GameMatchesPlayers>
    {
        public void Configure(EntityTypeBuilder<GameMatchesPlayers> builder)
        {
            builder.HasKey(x => x.Id)
                   .IsClustered();

            builder.HasOne(x => x.Game)
                   .WithMany(x => x.GameMatchesPlayers)
                   .HasForeignKey(x => x.GameId);

            builder.HasOne(x => x.Player)
                   .WithMany(x => x.GameMatchesPlayers)
                   .HasForeignKey(x => x.PlayerId);

            builder.Property(x => x.MatchDate)
                   .HasColumnType("datetimeoffset")
                   .IsRequired();

            builder.HasIndex(x => x.PlayerId)
                   .HasDatabaseName($"IX_{nameof(GameMatchesPlayers)}_{nameof(Player)}Id");
        }
    }
}
