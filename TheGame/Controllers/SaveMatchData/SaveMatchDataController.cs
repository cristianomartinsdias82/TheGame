using MediatR;
using TheGame.Controllers.Abstractions;

namespace TheGame.Controllers.SaveMatchData
{
    public class SaveMatchDataController : TheGameController
    {
        public SaveMatchDataController(IMediator mediator) : base(mediator) { }
    }
}
