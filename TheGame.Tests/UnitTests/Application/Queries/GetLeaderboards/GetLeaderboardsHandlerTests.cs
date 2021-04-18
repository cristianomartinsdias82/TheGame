using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel;
using TheGame.SharedKernel.Validation;
using Xunit;

namespace TheGame.Tests.UnitTests.Application.Queries.GetLeaderboards
{
    public class GetLeaderboardsHandlerTests
    {
        private readonly Mock<ITheGameCacheProvider> _cacheProviderMock;
        private readonly Mock<IDataInputValidation<GetLeaderboardsRequest>> _dataInputValidatorMock;

        public GetLeaderboardsHandlerTests()
        {
            _cacheProviderMock = new Mock<ITheGameCacheProvider>();
            _dataInputValidatorMock = new Mock<IDataInputValidation<GetLeaderboardsRequest>>();
        }

        [Fact]
        public async Task Should_fail_when_no_cancellation_token_is_invalid()
        {
            //Arrange
            var sut = new GetLeaderboardsHandler(_cacheProviderMock.Object, _dataInputValidatorMock.Object);

            //Act
            var response = await sut.Handle(new GetLeaderboardsRequest(), CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.False(response.Result.Succeeded);
            _dataInputValidatorMock.Verify(x => x.TryValidateAsync(It.IsAny<GetLeaderboardsRequest>(), It.IsAny<CancellationToken>()), Times.Never());
            _cacheProviderMock.Verify(x => x.GetPlayersListAsync(It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task Should_fail_when_data_input_has_invalid_arguments()
        {
            using (var cts = new CancellationTokenSource())
            {
                //Arrange
                var token = cts.Token;
                var sut = new GetLeaderboardsHandler(_cacheProviderMock.Object, _dataInputValidatorMock.Object);
                var request = new GetLeaderboardsRequest();
                
                _dataInputValidatorMock
                    .Setup(x => x.TryValidateAsync(It.Is<GetLeaderboardsRequest>(x => x == request), It.Is<CancellationToken>(x => x == token)))
                    .ReturnsAsync(OperationResult.Failure(It.IsAny<FailureDetail>()));
                
                //Act
                var response = await sut.Handle(request, token);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response.Result);
                Assert.False(response.Result.Succeeded);
                _dataInputValidatorMock.Verify(x => x.TryValidateAsync(request, It.Is<CancellationToken>(x => x == token)), Times.Once());
                _cacheProviderMock.Verify(x => x.GetLeaderboardsAsync(It.Is<CancellationToken>(x => x == token)), Times.Never());
            }
        }

        [Fact]
        public async Task Should_succeed_when_data_input_is_valid()
        {
            using (var cts = new CancellationTokenSource())
            {
                //Arrange
                var token = cts.Token;
                var sut = new GetLeaderboardsHandler(_cacheProviderMock.Object, _dataInputValidatorMock.Object);
                var request = new GetLeaderboardsRequest();

                _dataInputValidatorMock
                    .Setup(x => x.TryValidateAsync(It.Is<GetLeaderboardsRequest>(x => x == request), It.Is<CancellationToken>(x => x == token)))
                    .ReturnsAsync(OperationResult.Successful());

                _cacheProviderMock.Setup(x => x.GetLeaderboardsAsync(token))
                    .ReturnsAsync(It.IsAny<IEnumerable<PlayerBalanceDto>>());

                //Act
                var response = await sut.Handle(request, token);

                //Assert
                Assert.NotNull(response);
                Assert.NotNull(response.Result);
                Assert.True(response.Result.Succeeded);
                _dataInputValidatorMock.Verify(x => x.TryValidateAsync(request, It.Is<CancellationToken>(x => x == token)), Times.Once());
                _cacheProviderMock.Verify(x => x.GetLeaderboardsAsync(It.Is<CancellationToken>(x => x == token)), Times.Once());
            }
        }
    }
}
