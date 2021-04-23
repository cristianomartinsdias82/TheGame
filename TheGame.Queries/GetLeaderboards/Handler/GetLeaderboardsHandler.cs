using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Queries.GetLeaderboards
{
    internal class GetLeaderboardsHandler : IRequestHandler<GetLeaderboardsRequest, GetLeaderboardsResponse>
    {
        private readonly ITheGameCacheProvider _cacheProvider;

        public GetLeaderboardsHandler(ITheGameCacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
        }

        public async Task<GetLeaderboardsResponse> Handle(GetLeaderboardsRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
                return new GetLeaderboardsResponse
                {
                    Result = OperationResult<IEnumerable<PlayerBalanceDto>>.Failure(new FailureDetail("CancellationToken", "CancellationToken argument cannot be null."))
                };

            var leaderboards = await _cacheProvider.GetLeaderboardsAsync(cancellationToken);

            return new GetLeaderboardsResponse
            {
                Result = OperationResult<IEnumerable<PlayerBalanceDto>>.Successful(leaderboards ?? new PlayerBalanceDto[0])
            };
        }
    }
}
