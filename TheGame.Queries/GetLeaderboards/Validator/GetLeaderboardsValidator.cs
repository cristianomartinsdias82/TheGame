using FluentValidation;

namespace TheGame.Queries.GetLeaderboards
{
    public class GetLeaderboardsValidator : AbstractValidator<GetLeaderboardsRequest>
    {
        public GetLeaderboardsValidator()
        {
            RuleFor(x => x.PlayersMaxQuantity)
                .GreaterThan(0)
                .WithMessage("Invalid PlayersMaxQuantity argument");
        }
    }
}
