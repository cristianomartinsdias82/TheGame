using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.SharedKernel;
using TheGame.SharedKernel.Validation;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Queries.GetLeaderboards
{
    internal class GetLeaderboardsHandler : IRequestHandler<GetLeaderboardsRequest, GetLeaderboardsResponse>
    {
        private readonly ITheGameCacheProvider _cacheProvider;
        private readonly IDataInputValidation<GetLeaderboardsRequest> _requestValidator;

        public GetLeaderboardsHandler(
            ITheGameCacheProvider cacheProvider,
            IDataInputValidation<GetLeaderboardsRequest> requestValidator)
        {
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _requestValidator = requestValidator ?? throw ArgNullEx(nameof(requestValidator));
        }

        public async Task<GetLeaderboardsResponse> Handle(GetLeaderboardsRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
                return new GetLeaderboardsResponse
                {
                    Result = OperationResult<IEnumerable<PlayerBalanceDto>>.Failure(new FailureDetail("CancellationToken", "CancellationToken argument cannot be null."))
                };

            var validationResult = await _requestValidator.TryValidateAsync(request, cancellationToken);
            if (!validationResult.Succeeded)
                return new GetLeaderboardsResponse
                {
                    Result = OperationResult<IEnumerable<PlayerBalanceDto>>.Failure(validationResult.FailureDetails.ToArray())
                };

            var leaderboards = await _cacheProvider.GetLeaderboardsAsync(cancellationToken);

            return new GetLeaderboardsResponse
            {
                Result = OperationResult<IEnumerable<PlayerBalanceDto>>.Successful(leaderboards ?? new PlayerBalanceDto[0])
            };
        }
    }
}
