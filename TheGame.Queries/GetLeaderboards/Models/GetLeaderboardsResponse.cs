using System.Collections.Generic;
using TheGame.Common.Dto;
using TheGame.SharedKernel;

namespace TheGame.Queries.GetLeaderboards
{
    public class GetLeaderboardsResponse : ApplicationResponse<IEnumerable<PlayerBalanceDto>>
    {
    }
}
