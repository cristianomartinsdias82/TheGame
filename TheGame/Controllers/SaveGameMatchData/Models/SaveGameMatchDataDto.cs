using System;

namespace TheGame.Controllers.SaveMatchData
{
    public class SaveGameMatchDataDto
    {
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
