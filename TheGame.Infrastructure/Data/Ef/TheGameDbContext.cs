using Microsoft.EntityFrameworkCore;
using TheGame.Domain;

namespace TheGame.Data.Ef
{
    public class TheGameDbContext : DbContext
    {
        public TheGameDbContext() { }

        public TheGameDbContext(DbContextOptions<TheGameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameMatch> GameMatches { get; set; }
        public DbSet<GameMatchesPlayers> GameMatchesPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TheGameDbContext).Assembly);
        }
    }
}