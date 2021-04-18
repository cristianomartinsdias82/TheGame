using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel.Validation;
using Xunit;

namespace TheGame.Tests.UnitTests.Application.Queries.GetLeaderboards
{
    public class GetLeaderboardsValidatorTests
    {
        private readonly IDataInputValidation<GetLeaderboardsRequest> _sut;

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
                var result = await _sut.TryValidateAsync(new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity }, cts.Token);

                //Assert
                Assert.NotNull(result);
                Assert.False(result.Succeeded);
                Assert.NotNull(result.FailureDetails);
                Assert.True(result.FailureDetails.Count() > 0);
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
                var result = await _sut.TryValidateAsync(new GetLeaderboardsRequest { PlayersMaxQuantity = playersMaxQuantity }, cts.Token);

                //Assert
                Assert.NotNull(result);
                Assert.True(result.Succeeded);
                Assert.Null(result.FailureDetails);
            }
        }
    }
}
