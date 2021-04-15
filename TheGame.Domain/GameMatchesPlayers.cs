using System;
using TheGame.SharedKernel;

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

        public static OperationResult<GameMatchesPlayers> Create(long playerId, long matchId, DateTimeOffset matchDate, long win)
        => OperationResult<GameMatchesPlayers>.Successful
            (new GameMatchesPlayers
                {
                    GameMatchId = matchId,
                    PlayerId = playerId,
                    MatchDate = matchDate,
                    Win = win
                });
    }
}
