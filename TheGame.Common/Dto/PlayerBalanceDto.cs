using System;

namespace TheGame.Common.Dto
{
    public class PlayerBalanceDto
    {
        public string PlayerId { get; set; }
        public long Balance { get; set; }
        public DateTime ScoreLastUpdateOn { get; set; }
    }
}
