using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Queries.GetLeaderboards
{
    public class GetLeaderboardsHandler : IRequestHandler<GetLeaderboardsRequest, GetLeaderboardsResponse>
    {
        private readonly IReadonlyGameMatchRepository _repository;
        private readonly ICacheProvider _cacheProvider;

        public GetLeaderboardsHandler(
            IReadonlyGameMatchRepository repository,
            ICacheProvider cacheProvider)
        {
            _repository = repository ?? throw ArgNullEx(nameof(repository));
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
        }

        public async Task<GetLeaderboardsResponse> Handle(GetLeaderboardsRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult<GetLeaderboardsResponse>(default);
        }
    }
}
