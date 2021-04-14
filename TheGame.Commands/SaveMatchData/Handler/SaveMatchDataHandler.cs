using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.Common.SystemClock;
using TheGame.Domain;
using TheGame.SharedKernel;
using TheGame.SharedKernel.Validation;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Commands.SaveMatchData
{
    internal class SaveMatchDataHandler : IRequestHandler<SaveMatchDataRequest, SaveMatchDataResponse>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ILogger<SaveMatchDataHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDataInputValidation<SaveMatchDataRequest> _requestValidator;
        private readonly TheGameSettings _settings;
        private const string PlayerNotFound = "The informed player was not found.";

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public SaveMatchDataHandler(
            ICacheProvider cacheProvider,
            ILogger<SaveMatchDataHandler> logger,
            IDateTimeProvider dateTimeProvider,
            IDataInputValidation<SaveMatchDataRequest> requestValidator,
            TheGameSettings settings)
        {
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _logger = logger ?? throw ArgNullEx(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw ArgNullEx(nameof(dateTimeProvider));
            _requestValidator = requestValidator ?? throw ArgNullEx(nameof(requestValidator));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task<SaveMatchDataResponse> Handle(SaveMatchDataRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
                return new SaveMatchDataResponse { Result = OperationResult.Failure(new FailureDetail("CancellationToken", "CancellationToken argument cannot be null.")) };

            var validationResult = await _requestValidator.TryValidateAsync(request, cancellationToken);
            if (!validationResult.Succeeded)
                return new SaveMatchDataResponse { Result = validationResult };

            var players = await _cacheProvider.GetAsync<IEnumerable<Player>>(_settings.PlayersListCacheKey, cancellationToken);
            if ((players?.Count() ?? 0) == 0 || !players.Any(p => p.Id == request.PlayerId))
                return new SaveMatchDataResponse { Result = OperationResult.Failure(PlayerNotFound) };

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var cachedMatches = await _cacheProvider.GetAsync<IList<MatchDataDto>>(_settings.GameMatchesDataCacheKey, cancellationToken);
                if (cachedMatches == null)
                {
                    var matches = new List<MatchDataDto>();
                    matches.Add(MapFrom(request));
                    await _cacheProvider.SetAsync(matches, _settings.GameMatchesDataCacheKey, null, cancellationToken);
                }
                else
                {
                    cachedMatches.Add(MapFrom(request));
                    await _cacheProvider.SetAsync(cachedMatches, _settings.GameMatchesDataCacheKey, null, cancellationToken);
                }
            }
            catch(Exception exc)
            {
                var errorMessage = $"{_dateTimeProvider.DateTime:o} - Error while attempting to save match data";
                _logger.LogError(exc, errorMessage);

                return new SaveMatchDataResponse
                {
                    Result = OperationResult.Failure(errorMessage)
                };
            }
            finally
            {
                _semaphore.Release();
            }

            return new SaveMatchDataResponse
            {
                Result = OperationResult.Successful()
            };
        }

        private static MatchDataDto MapFrom(SaveMatchDataRequest request)
            => new MatchDataDto
            {
                MatchId = request.MatchId,
                MatchDate = request.MatchDate,
                PlayerId = request.PlayerId,
                Win = request.Win
            };
    }
}