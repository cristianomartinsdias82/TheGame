using System;

namespace TheGame.Domain
{
    public class GameMatchesPlayers
    {
        public long GameMatchId { get; set; }
        public GameMatch GameMatch { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}
