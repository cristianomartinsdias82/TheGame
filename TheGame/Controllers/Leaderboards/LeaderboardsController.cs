using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Dto;
using TheGame.Controllers.Abstractions;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Controllers.GetLeaderboards
{
    [TheGameRoute("leaderboards")]
    public class LeaderboardsController : TheGameController
    {
        private readonly TheGameSettings _settings;

        public LeaderboardsController(
            IMediator mediator,
            TheGameSettings settings) : base(mediator)
        {
            _settings = settings ?? throw ArgNullEx(nameof(settings));
        }

        /// <summary>
        /// Leaderboards endpoint
        /// </summary>
        /// <response code="200">Retrieves the OK status code along with the leaderboards</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [HttpGet]
        [ProducesResponseType(typeof(OperationResult<IEnumerable<PlayerBalanceDto>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult<IEnumerable<PlayerBalanceDto>>))]
        public async Task<ActionResult<OperationResult<IEnumerable<PlayerBalanceDto>>>> Leaderboards(
            [FromQuery] int? playersMaxQuantity,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity ?? _settings.TopMostRankedPlayersMaxQuantity },
                cancellationToken);

            var responseResult = response.GetResult();
            if (responseResult.Succeeded)
                return Ok(responseResult);

            return BadRequest(responseResult);
        }
    }
}
