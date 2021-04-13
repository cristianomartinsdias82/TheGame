using System.Collections.Generic;

namespace TheGame.Domain
{
    public class Player : Entity<long>
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public ICollection<GameMatchesPlayers> GameMatchesPlayers { get; set; }
    }
}
