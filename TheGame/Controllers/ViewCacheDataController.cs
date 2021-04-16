using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Controllers.Abstractions;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Controllers
{
    [TheGameRoute("cache-data")]
    public class ViewCacheDataController : TheGameController
    {
        private readonly ICacheProvider _cache;
        private readonly TheGameSettings _settings;

        public ViewCacheDataController(
            ICacheProvider cache,
            TheGameSettings settings,
            IMediator mediator) : base(mediator)
        {
            _cache = cache ?? throw ArgNullEx(nameof(cache));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        /// <summary>
        /// Retrieves a list of players
        /// </summary>
        /// <response code="200">Retrieves a list of players</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [Route("players-list")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<long>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<long>>))]
        [HttpGet]
        public async Task<ActionResult<OperationResult<IEnumerable<long>>>> GetPlayersList(CancellationToken cancellationToken)
        {
            return Ok(OperationResult<IEnumerable<long>>.Successful(await _cache.GetAsync<IEnumerable<long>>(_settings.PlayersListCacheKey, cancellationToken)));
        }

        /// <summary>
        /// Retrieves a list of games
        /// </summary>
        /// <response code="200">Retrieves a list of games</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [Route("games-list")]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<long>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<long>>))]
        [HttpGet]
        public async Task<ActionResult<OperationResult<IEnumerable<long>>>> GetGamesList(CancellationToken cancellationToken)
        {
            return Ok(OperationResult<IEnumerable<long>>.Successful(await _cache.GetAsync<IEnumerable<long>>(_settings.GamesListCacheKey, cancellationToken)));
        }
    }
}
