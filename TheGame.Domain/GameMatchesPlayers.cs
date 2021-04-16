using System;
using TheGame.SharedKernel;

namespace TheGame.Domain
{
    public class GameMatchesPlayers : Entity<long>
    {
        public long GameId { get; set; }
        public Game Game { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long Win { get; set; }
        public DateTimeOffset MatchDate { get; set; }

        public static OperationResult<GameMatchesPlayers> Create(long gameId, long playerId, long win, DateTimeOffset matchDate)
        => OperationResult<GameMatchesPlayers>.Successful
            (new GameMatchesPlayers
            {
                GameId = gameId,
                PlayerId = playerId,
                Win = win,
                MatchDate = matchDate
            });
    }
}
