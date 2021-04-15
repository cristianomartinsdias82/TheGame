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
    public class SaveMatchDataController : TheGameController
    {
        public SaveMatchDataController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<OperationResult>> SaveMatchData(
            [FromBody] SaveMatchDataDto request,
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
                return StatusCode((int)HttpStatusCode.Created);

            return BadRequest(response.Result.FailureDetails);
        }
    }
}
