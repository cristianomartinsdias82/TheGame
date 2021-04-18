using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.Controllers.Abstractions;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Controllers
{
    [TheGameRoute("cache-data")]
    //[Microsoft.AspNetCore.Authorization.Authorize]
    public class ViewCacheDataController : TheGameController
    {
        private readonly ITheGameCacheProvider _cacheProvider;
        private readonly TheGameSettings _settings;

        public ViewCacheDataController(
            ITheGameCacheProvider cacheProvider,
            TheGameSettings settings,
            IMediator mediator) : base(mediator)
        {
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        /// <summary>
        /// Retrieves a list of players
        /// </summary>
        /// <response code="200">Retrieves a list of players</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [Route("players")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<long>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<long>>))]
        [HttpGet]
        public async Task<ActionResult<OperationResult<IEnumerable<long>>>> Players(CancellationToken cancellationToken)
        {
            return Ok(OperationResult<IEnumerable<long>>.Successful(await _cacheProvider.GetPlayersListAsync(cancellationToken)));
        }

        /// <summary>
        /// Retrieves a list of games
        /// </summary>
        /// <response code="200">Retrieves a list of games</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [Route("games")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<long>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<long>>))]
        [HttpGet]
        public async Task<ActionResult<OperationResult<IEnumerable<long>>>> Games(CancellationToken cancellationToken)
        {
            return Ok(OperationResult<IEnumerable<long>>.Successful(await _cacheProvider.GetGamesListAsync(cancellationToken)));
        }

        /// <summary>
        /// Retrieves a list of game matches
        /// </summary>
        /// <response code="200">Retrieves a list of game matches</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [Route("game-matches")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<CacheItem<GameMatchDataDto>>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<CacheItem<GameMatchDataDto>>>))]
        [HttpGet]
        public async Task<ActionResult<OperationResult<IEnumerable<CacheItem<GameMatchDataDto>>>>> GameMatches(CancellationToken cancellationToken)
        {
            return Ok(OperationResult<IEnumerable<CacheItem<GameMatchDataDto>>>.Successful(await _cacheProvider.GetGameMatchesAsync(cancellationToken)));
        }
    }
}
