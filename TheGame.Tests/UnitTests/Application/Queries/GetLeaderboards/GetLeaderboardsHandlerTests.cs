using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.Queries.GetLeaderboards;
using Xunit;

namespace TheGame.Tests.UnitTests.Application.Queries.GetLeaderboards
{
    public class GetLeaderboardsHandlerTests
    {
        private readonly Mock<ITheGameCacheProvider> _cacheProviderMock;

        public GetLeaderboardsHandlerTests()
        {
            _cacheProviderMock = new Mock<ITheGameCacheProvider>();
        }

        [Fact]
        public async Task Should_fail_when_no_cancellation_token_is_provided()
        {
            //Arrange
            var sut = new GetLeaderboardsHandler(_cacheProviderMock.Object);

            //Act
            var response = await sut.Handle(new GetLeaderboardsRequest(), CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.False(response.Result.Succeeded);
            _cacheProviderMock.Verify(x => x.GetPlayersListAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task Should_succeed_when_data_input_is_valid()
        {
            using (var cts = new CancellationTokenSource())
            {
                //Arrange
                var token = cts.Token;
                var sut = new GetLeaderboardsHandler(_cacheProviderMock.Object);
                var request = new GetLeaderboardsRequest();

                _cacheProviderMock
                    .Setup(x => x.GetLeaderboardsAsync(token))
                    .ReturnsAsync(It.IsAny<IEnumerable<PlayerBalanceDto>>());

                //Act
                var response = await sut.Handle(request, token);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response.Result);
                Assert.True(response.Result.Succeeded);
                _cacheProviderMock.Verify(x => x.GetLeaderboardsAsync(It.Is<CancellationToken>(x => x == token)), Times.Once());
            }
        }
    }
}
