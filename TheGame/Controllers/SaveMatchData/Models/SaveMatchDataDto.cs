namespace TheGame.Controllers.SaveMatchData.Models
{
    public class SaveMatchDataDto
    {
        public string PlayerName { get; set; }
        public string PlayerNickname { get; set; }
        public long MatchId { get; set; }
        public long Win { get; set; }
    }
}
