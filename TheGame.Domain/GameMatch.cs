using System.Collections.Generic;

namespace TheGame.Domain
{
    public class GameMatch : Entity<long>
    {
        public string Title { get; set; }
        public ICollection<GameMatchesPlayers> GameMatchesPlayers { get; set; }
    }
}
