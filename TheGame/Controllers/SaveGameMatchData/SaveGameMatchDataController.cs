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
        /// <response code="201">Retrieves the Created status code</response>
        /// <response code="400">Retrieves the Bad Request status code along with a failed operation result object</response>
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(OperationResult))]
        [HttpPost]
        public async Task<ActionResult<OperationResult>> SaveGameMatchData(
            [FromBody] SaveGameMatchDataDto request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new SaveGameMatchDataRequest
                {
                    MatchDate = request.Timestamp,
                    GameId = request.GameId,
                    PlayerId = request.PlayerId,
                    Win = request.Win
                },
                cancellationToken);

            var result = response.GetResult();
            if (result.Succeeded)
                return StatusCode((int)HttpStatusCode.Created, result);

            return BadRequest(result);
        }
    }
}
