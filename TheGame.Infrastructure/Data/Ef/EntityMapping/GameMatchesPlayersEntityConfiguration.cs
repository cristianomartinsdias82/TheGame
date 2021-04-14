using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    public class GameMatchesPlayersEntityConfiguration : IEntityTypeConfiguration<GameMatchesPlayers>
    {
        public void Configure(EntityTypeBuilder<GameMatchesPlayers> builder)
        {
            builder.HasKey(x => new { x.GameMatchId, x.PlayerId })
                   .IsClustered();

            builder.HasOne(x => x.GameMatch)
                   .WithMany(x => x.GameMatchesPlayers)
                   .HasForeignKey(x => x.GameMatchId);

            builder.HasOne(x => x.Player)
                   .WithMany(x => x.GameMatchesPlayers)
                   .HasForeignKey(x => x.PlayerId);

            builder.Property(x => x.MatchDate)
                   .HasColumnType("datetimeoffset")
                   .IsRequired();
        }
    }
}
