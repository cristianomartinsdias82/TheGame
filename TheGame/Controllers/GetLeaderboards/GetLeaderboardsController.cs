using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Controllers.Abstractions;
using TheGame.SharedKernel;
using TheGame.Queries.GetLeaderboards;

namespace TheGame.Controllers.GetLeaderboards
{
    public class GetLeaderboardsController : TheGameController
    {
        public GetLeaderboardsController(IMediator mediator) : base(mediator) { }

        [HttpGet("/api/leaderboards")]
        public async Task<ActionResult<OperationResult<GetLeaderboardsDto>>> GetLeaderboards(
            [FromQuery] int playersMaxQuantity,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity },
                cancellationToken);

            if (response.Result.Succeeded)
                return Ok(response.Result);

            return BadRequest(response.Result.FailureDetails);
        }
    }
}
