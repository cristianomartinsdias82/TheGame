using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Commands.Repositories;
using TheGame.Common.Caching;
using TheGame.Common.SystemClock;
using TheGame.Domain;
using TheGame.MatchDataFlushingWorker.Utilities;
using TheGame.Queries.GetLeaderboards.Repositories;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;
using TheGame.SharedKernel;

namespace TheGame.MatchDataFlushingWorker
{
    public class GameMatchesDataDbFlushingService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GameMatchesDataDbFlushingService> _logger;
        private readonly ICacheProvider _cacheProvider;
        private readonly ITheGameCommandsRepository _commandsRepository;
        private readonly ITheGameQueriesRepository _queriesRepository;
        private readonly TheGameSettings _settings;
        private readonly IDateTimeProvider _dateTime;
        private Timer _timer;
        private Random _random = new Random();
        private CancellationTokenSource _cancellationTokenSource;
        private Task _ongoingTask;

        public GameMatchesDataDbFlushingService(
            IConfiguration configuration,
            ILogger<GameMatchesDataDbFlushingService> logger,
            ICacheProvider cacheProvider,
            ITheGameCommandsRepository commandsRepository,
            ITheGameQueriesRepository queriesRepository,
            TheGameSettings settings,
            IDateTimeProvider dateTime)
        {
            _configuration = configuration ?? throw ArgNullEx(nameof(configuration));
            _logger = logger ?? throw ArgNullEx(nameof(logger));
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _commandsRepository = commandsRepository ?? throw ArgNullEx(nameof(commandsRepository));
            _queriesRepository = queriesRepository ?? throw ArgNullEx(nameof(queriesRepository));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
            _dateTime = dateTime ?? throw ArgNullEx(nameof(dateTime));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            ScheduleJobExecution();

            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - {nameof(GameMatchesDataDbFlushingService)} started.");

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - {nameof(GameMatchesDataDbFlushingService)} stopped.");

            _cancellationTokenSource.Cancel();

            await Task.WhenAny(_ongoingTask, Task.Delay(-1, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task ExecuteAsync(object state, CancellationToken cancellationToken)
        {
            var bulkOperationsSuccessful = await PerformBulkOperationsAsync(cancellationToken);
            if (bulkOperationsSuccessful)
                await RefreshLeaderboardsAsync(cancellationToken);

            ScheduleJobExecution();
        }

        private async Task<bool> PerformBulkOperationsAsync(CancellationToken cancellationToken)
        {
            var dataFlushingSuccessful = false;
            var matchData = await _cacheProvider.GetMatchDataAsync(_settings, cancellationToken);

            if (matchData?.Any() ?? false)
            {
                var now = _dateTime.DateTimeOffset.ConvertUtcToLocalDateTime(_settings);
                var gameMatchesPlayers = matchData.Select(it => GameMatchesPlayers.Create(it.PlayerId, it.MatchId, it.MatchDate, it.Win).Data)
                                                  .ToList();

                var players = await _queriesRepository.FetchPlayersByIdsAsync(matchData.Select(x => x.PlayerId).Distinct(), cancellationToken);
                players.ToList().ForEach(it => it.ScoreLastUpdateOn = now);

                dataFlushingSuccessful = await TransactionContextHelper.Execute(async (matchData) =>
                {
                    _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Flushing data to database...");

                    await _commandsRepository.BulkInsertGameMatchesPlayersAsync(gameMatchesPlayers, cancellationToken);

                    await _commandsRepository.BulkUpdatePlayersAsync(players, cancellationToken);

                    await _cacheProvider.ClearAsync(_settings.GameMatchesDataCacheKey, cancellationToken);

                    _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Data flushing executed successfully!");

                    return true;
                },
                matchData,
                fromSecondsTransactionTimeout: Math.Abs(_settings.BatchOperationsTimeout),
                throwOnError: false);
            }

            return dataFlushingSuccessful;
        }

        private async Task RefreshLeaderboardsAsync(CancellationToken cancellationToken)
        {
            var leaderboards = await _queriesRepository.FetchLeaderboardsAsync(Math.Abs(_settings.TopMostRankedPlayersMaxQuantity), cancellationToken);
            await _cacheProvider.SetAsync(leaderboards, _settings.LeaderboardsCacheKey, null, cancellationToken);
        }

        private void ScheduleJobExecution()
        {
            _ongoingTask = new Task(async (state) => await ExecuteAsync(state, _cancellationTokenSource.Token), _cancellationTokenSource.Token);

            //_timer = new Timer(async (state) => { await ExecuteAsync(state, _cancellationTokenSource.Token); },
            _timer = new Timer((state) => _ongoingTask.Start(),
                               null,
                               //TimeSpan.FromSeconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(-1));
        }
    }
}