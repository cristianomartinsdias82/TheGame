IF OBJECT_ID('dbo.V_Leaderboards') IS NOT NULL
	DROP VIEW dbo.V_Leaderboards
GO

CREATE VIEW dbo.V_Leaderboards
AS
	SELECT
		 PL.Id PlayerId
		,SUM(GMP.Win) Balance
		,PL.ScoreLastUpdateOn
	FROM dbo.Players PL
	INNER JOIN dbo.GameMatchesPlayers GMP ON PL.Id = GMP.PlayerId
	GROUP BY
		 PL.Id
		,PL.ScoreLastUpdateOn
GO