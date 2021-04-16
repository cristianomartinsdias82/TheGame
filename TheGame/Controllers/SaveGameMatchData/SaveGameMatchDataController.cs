using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Commands.SaveMatchData;
using TheGame.Controllers.Abstractions;
using TheGame.SharedKernel;

namespace TheGame.Controllers.SaveMatchData
{
    [TheGameRoute("match")]
    public class SaveGameMatchDataController : TheGameController
    {
        public SaveGameMatchDataController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Save game match data endpoint
        /// </summary>
        /// <response code="200">Retrieves the leaderboards</response>
        /// <response code="400">Retrieves an error list</response>
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(OperationResult))]
        [HttpPost]
        public async Task<ActionResult<OperationResult>> SaveGameMatchData(
            [FromBody] SaveGameMatchDataDto request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new SaveMatchDataRequest
                {
                    MatchDate = request.MatchDate,
                    MatchId = request.MatchId,
                    PlayerId = request.PlayerId,
                    Win = request.Win
                },
                cancellationToken);

            if (response.Result.Succeeded)
                return StatusCode((int)HttpStatusCode.Created, response.Result);

            return BadRequest(response.Result);
        }
    }
}
