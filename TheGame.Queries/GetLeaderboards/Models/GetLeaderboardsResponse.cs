using System.Collections.Generic;
using TheGame.Common.Dto;
using TheGame.SharedKernel;

namespace TheGame.Queries.GetLeaderboards
{
    public class GetLeaderboardsResponse
    {
        public OperationResult<IEnumerable<PlayerBalanceDto>> Result { get; set; }
    }
}
