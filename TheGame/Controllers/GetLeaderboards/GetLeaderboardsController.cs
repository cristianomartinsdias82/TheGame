using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Controllers.Abstractions;
using TheGame.SharedKernel;

namespace TheGame.Controllers.GetLeaderboards
{
    public class GetLeaderboardsController : TheGameController
    {
        public GetLeaderboardsController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<ActionResult<OperationResult<GetLeaderboardsDto>>> GetLeaderboards(CancellationToken cancellationToken)
        {
            return await Task.FromResult<OperationResult<GetLeaderboardsDto>>(default);
        }
    }
}
