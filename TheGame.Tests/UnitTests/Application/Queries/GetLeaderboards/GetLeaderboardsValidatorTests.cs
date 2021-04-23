using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Queries.GetLeaderboards;
using Xunit;

namespace TheGame.Tests.UnitTests.Application.Queries.GetLeaderboards
{
    public class GetLeaderboardsValidatorTests
    {
        private readonly AbstractValidator<GetLeaderboardsRequest> _sut;

        public GetLeaderboardsValidatorTests()
        {
            _sut = new GetLeaderboardsValidator();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Should_fail_when_players_max_quantity_argument_is_less_than_or_equal_to_zero(int playersMaxQuantity)
        {
            using (var cts = new CancellationTokenSource())
            {
                //Act
                var result = await _sut.ValidateAsync(new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity }, cts.Token);

                //Assert
                Assert.NotNull(result);
                Assert.False(result.IsValid);
                Assert.NotNull(result.Errors);
                Assert.True(result.Errors.Any());
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public async Task Should_succeed_when_players_max_quantity_argument_is_greater_than_zero(int playersMaxQuantity)
        {
            using (var cts = new CancellationTokenSource())
            {
                //Act
                var result = await _sut.ValidateAsync(new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity }, cts.Token);

                //Assert
                Assert.NotNull(result);
                Assert.True(result.IsValid);
                Assert.True(result.Errors == null || !result.Errors.Any());
            }
        }
    }
}
