namespace TheGame.SharedKernel
{
    public class TheGameSettings
    {
        public string Title { get; set; }
        public string CurrentVersion { get; set; }
        public string DataProviderName { get; set; }
        public string DbConnectionString { get; set; }
        public string ObjectsSchema { get; set; }
        public int BatchOperationsTimeoutInSecs { get; set; }
        public string Timezone { get; set; }
        public int TimeBetweenDataFlushingOperationsInSecs { get; set; }
        public string LeaderboardsCacheKey { get; set; }
        public string GameMatchesDataCacheKey { get; set; }
        public string PlayersListCacheKey { get; set; }
        public string GamesListCacheKey { get; set; }
        public int TopMostRankedPlayersMaxQuantity { get; set; }
        public long CacheSizeLimit { get; set; }
        public double CacheCompactionPecentage { get; set; }
    }
}
