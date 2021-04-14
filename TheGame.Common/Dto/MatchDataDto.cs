
using System;

namespace TheGame.Common.Dto
{
    public class MatchDataDto
    {
        public long PlayerId { get; set; }
        public long MatchId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}
