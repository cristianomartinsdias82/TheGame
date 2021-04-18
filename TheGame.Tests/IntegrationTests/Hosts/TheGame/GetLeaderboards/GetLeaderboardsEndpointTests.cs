using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static TheGame.SharedKernel.Helpers.ExceptionHelper;

namespace TheGame.Tests.IntegrationTests.Hosts.TheGame.GetLeaderboards
{
    public class GetLeaderboardsEndpointTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;

        public GetLeaderboardsEndpointTests(CustomWebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory ?? throw ArgNullEx(nameof(webApplicationFactory));
            _httpClient = _webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Get_leaderboards_retrieves_non_null_cached_data_successfully()
        {
            using (var cts = new CancellationTokenSource())
            {
                var httpResponse = await _httpClient.GetAsync("/api/v1/leaderboards", cts.Token);
                var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

                Assert.True(httpResponse.IsSuccessStatusCode);
            }
        }
    }
}
