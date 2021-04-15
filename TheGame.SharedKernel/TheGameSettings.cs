namespace TheGame.SharedKernel
{
    public class TheGameSettings
    {
        public string DataProviderName { get; set; }
        public string DbConnectionString { get; set; }
        public string ObjectsSchema { get; set; }
        public int BatchOperationsTimeout { get; set; }
        public string Timezone { get; set; }
        public int TimeBetweenDataFlushingOperationsInSecs { get; set; }
        public int TimeBetweenLeaderboardsUpdatesInSecs { get; set; }
        public string LeaderboardsCacheKey { get; set; }
        public string GameMatchesDataCacheKey { get; set; }
        public string PlayersListCacheKey { get; set; }
        public int TopMostRankedPlayersMaxQuantity { get; set; }
    }
}
