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
    internal class SaveGameMatchDataHandler : IRequestHandler<SaveGameMatchDataRequest, SaveGameMatchDataResponse>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ILogger<SaveGameMatchDataHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDataInputValidation<SaveGameMatchDataRequest> _requestValidator;
        private readonly TheGameSettings _settings;
        private const string PlayerNotFound = "The informed player was not found.";
        private const string GameNotFound = "The informed game was not found.";

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public SaveGameMatchDataHandler(
            ICacheProvider cacheProvider,
            ILogger<SaveGameMatchDataHandler> logger,
            IDateTimeProvider dateTimeProvider,
            IDataInputValidation<SaveGameMatchDataRequest> requestValidator,
            TheGameSettings settings)
        {
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _logger = logger ?? throw ArgNullEx(nameof(logger));
            _dateTimeProvider = dateTimeProvider ?? throw ArgNullEx(nameof(dateTimeProvider));
            _requestValidator = requestValidator ?? throw ArgNullEx(nameof(requestValidator));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        public async Task<SaveGameMatchDataResponse> Handle(SaveGameMatchDataRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
                return new SaveGameMatchDataResponse { Result = OperationResult.Failure(new FailureDetail("CancellationToken", "CancellationToken argument cannot be null.")) };

            var validationResult = await _requestValidator.TryValidateAsync(request, cancellationToken);
            if (!validationResult.Succeeded)
                return new SaveGameMatchDataResponse { Result = validationResult };

            var players = await _cacheProvider.GetAsync<IEnumerable<Player>>(_settings.PlayersListCacheKey, cancellationToken);
            if ((players?.Count() ?? 0) == 0 || !players.Any(p => p.Id == request.PlayerId))
                return new SaveGameMatchDataResponse { Result = OperationResult.Failure(PlayerNotFound) };

            var games = await _cacheProvider.GetAsync<IEnumerable<Game>>(_settings.GamesListCacheKey, cancellationToken);
            if ((games?.Count() ?? 0) == 0 || !games.Any(p => p.Id == request.PlayerId))
                return new SaveGameMatchDataResponse { Result = OperationResult.Failure(GameNotFound) };

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                var cachedGameMatches = await _cacheProvider.GetAsync<IList<GameMatchDataDto>>(_settings.GameMatchesDataCacheKey, cancellationToken);
                if (cachedGameMatches == null)
                {
                    var matches = new List<GameMatchDataDto>();
                    matches.Add(MapFrom(request));
                    await _cacheProvider.SetAsync(matches, _settings.GameMatchesDataCacheKey, null, cancellationToken);
                }
                else
                {
                    cachedGameMatches.Add(MapFrom(request));
                    await _cacheProvider.SetAsync(cachedGameMatches, _settings.GameMatchesDataCacheKey, null, cancellationToken);
                }
            }
            catch(Exception exc)
            {
                var errorMessage = $"{_dateTimeProvider.DateTime:o} - Error while attempting to save match data";
                _logger.LogError(exc, errorMessage);

                return new SaveGameMatchDataResponse
                {
                    Result = OperationResult.Failure(errorMessage)
                };
            }
            finally
            {
                _semaphore.Release();
            }

            return new SaveGameMatchDataResponse
            {
                Result = OperationResult.Successful()
            };
        }

        private static GameMatchDataDto MapFrom(SaveGameMatchDataRequest request)
            => new GameMatchDataDto
            {
                PlayerId = request.PlayerId,
                GameId = request.GameId,
                Win = request.Win,
                MatchDate = request.MatchDate
            };
    }
}