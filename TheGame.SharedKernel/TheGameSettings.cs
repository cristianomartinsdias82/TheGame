namespace TheGame.SharedKernel
{
    public class TheGameSettings
    {
        public string DbConnectionString { get; set; }
        public int TimeBetweenDataFlushingOperationsInSecs { get; set; }
        public string LeaderboardsCacheKey { get; set; }
        public string GameMatchesDataCacheKey { get; set; }
        public string PlayersListCacheKey { get; set; }
    }
}
