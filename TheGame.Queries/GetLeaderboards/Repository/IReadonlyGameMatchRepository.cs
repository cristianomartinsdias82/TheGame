using System.Collections.Generic;

namespace TheGame.Queries.GetLeaderboards
{
    public interface IReadonlyGameMatchRepository
    {
        public IEnumerable<GameMatchDataDto> GetTopXPlayersByBalance(int top = 100);
    }
}
