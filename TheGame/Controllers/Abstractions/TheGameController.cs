using MediatR;
using Microsoft.AspNetCore.Mvc;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Controllers.Abstractions
{
    [ApiController]
    [TheGameRoute("[controller]")]
    public abstract class TheGameController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public TheGameController(IMediator mediator)
        {
            _mediator = mediator ?? throw ArgNullEx(nameof(mediator));
        }
    }
}
