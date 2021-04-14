using MediatR;

namespace TheGame.Queries.GetLeaderboards
{
    public class GetLeaderboardsRequest : IRequest<GetLeaderboardsResponse>
    {
        public int PlayersMaxQuantity { get; set; }
    }
}
