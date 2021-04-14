using TheGame.SharedKernel.Validation;
using FluentValidation;

namespace TheGame.Commands.SaveMatchData
{
    internal class SaveMatchDataValidator : DataInputValidator<SaveMatchDataRequest>
    {
        public SaveMatchDataValidator()
        {
            RuleFor(x => x.MatchId)
                .GreaterThan(0)
                .WithMessage("Invalid MatchId argument");

            RuleFor(x => x.PlayerId)
                .GreaterThan(0)
                .WithMessage("Invalid PlayerId argument");
        }
    }
}
