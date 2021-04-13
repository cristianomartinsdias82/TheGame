using System;

namespace TheGame.Queries.GetLeaderboards
{
    public class GameMatchDataDto
    {
        public long PlayerId { get; set; }
        public long Balance { get; set; }
        public DateTime LastUpdateOn { get; set; }
    }
}
