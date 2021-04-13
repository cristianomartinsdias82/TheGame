using Microsoft.EntityFrameworkCore;
using TheGame.Domain;

namespace TheGame.Infrastructure.Data.Ef
{
    public sealed class TheGameDataContext : DbContext
    {
        public TheGameDataContext(DbContextOptions<TheGameDataContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameMatch> GameMatches { get; set; }
        public DbSet<GameMatchesPlayers> GameMatchesPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
