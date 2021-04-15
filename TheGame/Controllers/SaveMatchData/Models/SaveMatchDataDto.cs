﻿using System;

namespace TheGame.Controllers.SaveMatchData
{
    public class SaveMatchDataDto
    {
        public long PlayerId { get; set; }
        public long MatchId { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }
    }
}