﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheGame.Commands.SaveMatchData;
using TheGame.Common.Caching;
using TheGame.Common.Dto;
using TheGame.Common.SystemClock;
using TheGame.Data.Ef;
using TheGame.Domain;
using TheGame.Infrastructure.Data;
using TheGame.Infrastructure.Data.Ef.Factory;
using TheGame.Queries.GetLeaderboards;
using TheGame.SharedKernel;
using static TheGame.SharedKernel.ExceptionHelper;

namespace TheGame.MatchDataFlushingWorker
{
    public class GameMatchesDataDbFlushingService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GameMatchesDataDbFlushingService> _logger;
        private readonly ITheGameCacheProvider _cacheProvider;
        private readonly ITheGameCommandsRepository _commandsRepository;
        private readonly ITheGameQueriesRepository _queriesRepository;
        private readonly TheGameSettings _settings;
        private readonly IDateTimeProvider _dateTime;
        private Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _ongoingTask;

        public GameMatchesDataDbFlushingService(
            IConfiguration configuration,
            ILogger<GameMatchesDataDbFlushingService> logger,
            ITheGameCacheProvider cacheProvider,
            TheGameSettings settings,
            IDateTimeProvider dateTime)
        {
            _configuration = configuration ?? throw ArgNullEx(nameof(configuration));
            _logger = logger ?? throw ArgNullEx(nameof(logger));
            _cacheProvider = cacheProvider ?? throw ArgNullEx(nameof(cacheProvider));
            _settings = settings ?? throw ArgNullEx(nameof(settings));
            _dateTime = dateTime ?? throw ArgNullEx(nameof(dateTime));

            _commandsRepository = RepositoryFactory.GetCommandsRepository(GetDbContext());
            _queriesRepository = RepositoryFactory.GetQueriesRepository(settings);
        }

        private TheGameDbContext GetDbContext()
            => new TheGameDbContextFactory().CreateDbContext(new string[] { $"connectionString={_settings.DbConnectionString}" });

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - {nameof(GameMatchesDataDbFlushingService)} has started.");

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            ScheduleJobExecution();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            await Task.WhenAny(_ongoingTask, Task.Delay(-1, cancellationToken));

            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - {nameof(GameMatchesDataDbFlushingService)} has stopped.");

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
            var matchData = (await _cacheProvider.GetGameMatchesAsync(cancellationToken))?.ToList();

            if (matchData?.Any() ?? false)
            {
                var now = _dateTime.DateTimeOffset.LocalDateTime;
                var gameMatchesPlayers = matchData
                                            .Select(it => GameMatchesPlayers.Create(
                                                            it.Item.GameId,
                                                            it.Item.PlayerId,
                                                            it.Item.Win,
                                                            it.Item.MatchDate).Data)
                                            .ToList();

                var players = await _queriesRepository.FetchPlayersByIdsAsync(matchData.Select(x => x.Item.PlayerId).Distinct(), cancellationToken);
                players.ToList().ForEach(it => it.ScoreLastUpdateOn = now);

                dataFlushingSuccessful = await TransactionContextHelper.Execute(async (matchData) =>
                {
                    _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Flushing data to database...");

                    await _commandsRepository.BulkInsertGameMatchesPlayersAsync(gameMatchesPlayers, cancellationToken);

                    await _commandsRepository.BulkUpdatePlayersAsync(players, cancellationToken);

                    _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Data flushing executed successfully!");

                    var lockTaken = false;
                    try
                    {
                        //Monitor.Enter(typeof(ITheGameCacheProvider), ref lockTaken);
                        //if (lockTaken)
                        if (lockTaken = Monitor.TryEnter(typeof(ITheGameCacheProvider), 10000))
                        {
                            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Removing flushed data from cache...");

                            var gameMatchDataCurrentSnapshot = _cacheProvider.GetGameMatchesAsync(cancellationToken).GetAwaiter().GetResult();
                            var pendingGameMatchData = gameMatchDataCurrentSnapshot.Except(matchData, new CachedMatchGameDataComparer());

                            _cacheProvider.StoreGameMatchesAsync(pendingGameMatchData, null, cancellationToken).GetAwaiter().GetResult();

                            _logger.LogInformation($"{_dateTime.DateTime:dd-MM-yyyy hh:mm:ss} - Flushed data successfully removed from cache!");
                        }
                    }
                    finally
                    {
                        if (lockTaken)
                            Monitor.Exit(typeof(ITheGameCacheProvider));
                    }

                    //lock (typeof(ITheGameCacheProvider))
                    //{
                    //    var gameMatchDataCurrentSnapshot = _cacheProvider.GetGameMatchesAsync(cancellationToken).GetAwaiter().GetResult();
                    //    var pendingGameMatchData = gameMatchDataCurrentSnapshot.Except(matchData, new CacheItemComparer());

                    //    _cacheProvider.StoreGameMatchesAsync(pendingGameMatchData, null, cancellationToken).GetAwaiter().GetResult();
                    //}

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

            _timer = new Timer((state) => _ongoingTask.Start(),
                               null,
                               //TimeSpan.FromSeconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(Math.Abs(_settings.TimeBetweenDataFlushingOperationsInSecs)),
                               TimeSpan.FromMilliseconds(-1));
        }
    }
}