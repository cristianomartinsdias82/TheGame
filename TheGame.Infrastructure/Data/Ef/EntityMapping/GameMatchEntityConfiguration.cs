using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef.EntityMapping
{
    public class GameMatchEntityConfiguration : IEntityTypeConfiguration<GameMatch>
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
        }
    }
}
