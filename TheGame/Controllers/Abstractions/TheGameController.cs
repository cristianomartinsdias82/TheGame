using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
