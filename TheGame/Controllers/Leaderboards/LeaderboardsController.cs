using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Controllers.Abstractions;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;

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

        [HttpGet]
        public async Task<ActionResult<OperationResult<LeaderboardsDto>>> Leaderboards(
            [FromQuery] int? playersMaxQuantity,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity ?? _settings.TopMostRankedPlayersMaxQuantity },
                cancellationToken);

            if (response.Result.Succeeded)
                return Ok(response.Result);

            return BadRequest(response.Result.FailureDetails);
        }
    }
}
