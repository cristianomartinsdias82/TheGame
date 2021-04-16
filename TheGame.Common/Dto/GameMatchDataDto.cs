
using System;

namespace TheGame.Common.Dto
{
    public class GameMatchDataDto
    {
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}
