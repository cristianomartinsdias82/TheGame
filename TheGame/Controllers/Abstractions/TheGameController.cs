using MediatR;
using Microsoft.AspNetCore.Mvc;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.Controllers.Abstractions
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class TheGameController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public TheGameController(IMediator mediator)
        {
            _mediator = mediator ?? throw ArgNullEx(nameof(mediator));
        }
    }
}
