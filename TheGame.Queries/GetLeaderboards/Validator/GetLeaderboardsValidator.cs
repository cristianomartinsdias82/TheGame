using FluentValidation;
using TheGame.SharedKernel.Validation;

namespace TheGame.Queries.GetLeaderboards
{
    internal class GetLeaderboardsValidator : DataInputValidator<GetLeaderboardsRequest>
    {
        public GetLeaderboardsValidator()
        {
            RuleFor(x => x.PlayersMaxQuantity)
                .GreaterThan(0)
                .WithMessage("Invalid PlayersMaxQuantity argument");
        }
    }
}
