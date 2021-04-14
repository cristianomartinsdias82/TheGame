using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    public class PlayerEntityConfiguration : IEntityTypeConfiguration<Player>
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

            builder.Property(x => x.ScoreLastUpdateOn)
                    .HasColumnType("datetimeoffset")
                    .IsRequired();

            builder.HasIndex(x => x.ScoreLastUpdateOn)
                   .HasDatabaseName($"IX_{nameof(Player)}_{nameof(Player.ScoreLastUpdateOn)}");
        }
    }
}
