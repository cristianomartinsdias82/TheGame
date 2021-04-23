using FluentValidation;

namespace TheGame.Commands.SaveMatchData
{
    public class SaveGameMatchDataValidator : AbstractValidator<SaveGameMatchDataRequest>
    {
        public SaveGameMatchDataValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0)
                .WithMessage("Invalid GameId argument");

            RuleFor(x => x.PlayerId)
                .GreaterThan(0)
                .WithMessage("Invalid PlayerId argument");
        }
    }
}
