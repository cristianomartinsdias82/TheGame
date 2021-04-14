using FluentValidation;
using TheGame.SharedKernel.Validation;

namespace TheGame.Queries.GetLeaderboards.Validator
{
    internal class GetLeaderboardsValidator : DataInputValidator<GetLeaderboardsRequest>
    {
        public GetLeaderboardsValidator()
        {
            RuleFor(x => x.MaxRecords)
                .GreaterThan(0)
                .WithMessage("Invalid MaxRecords argument");
        }
    }
}
