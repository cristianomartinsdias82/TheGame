using System.Collections.Generic;

namespace TheGame.Queries.GetLeaderboards
{
    public class LeaderboardsDto
    {
        public LeaderboardsDto()
        {
            Scoreboard = new List<GameMatchDataDto>();
        }

        public IEnumerable<GameMatchDataDto> Scoreboard { get; set; }
    }
}
