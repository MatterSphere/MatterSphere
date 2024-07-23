



CREATE PROCEDURE [dbo].[MatterMilestoneTasks] 
	-- Add the parameters for the stored procedure here
	@MATTER_ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;

DECLARE @IsMileStone int;
SET @IsMileStone = 1;

DECLARE @IsTask int;
SET @IsTask = 0;

WITH [OMS2K] AS(
SELECT 
	[dbMSData_OMS2K].[fileID]
	, TStage.TStage
	, MAX(CASE TStage.TStage 
			WHEN 1 THEN [dbMSConfig_OMS2K].MSStage1Desc 
			WHEN 2 THEN [dbMSConfig_OMS2K].MSStage2Desc 
			WHEN 3 THEN [dbMSConfig_OMS2K].MSStage3Desc 
			WHEN 4 THEN [dbMSConfig_OMS2K].MSStage4Desc 
			WHEN 5 THEN [dbMSConfig_OMS2K].MSStage5Desc 
			WHEN 6 THEN [dbMSConfig_OMS2K].MSStage6Desc 
			WHEN 7 THEN [dbMSConfig_OMS2K].MSStage7Desc 
			WHEN 8 THEN [dbMSConfig_OMS2K].MSStage8Desc 
			WHEN 9 THEN [dbMSConfig_OMS2K].MSStage9Desc 
			WHEN 10 THEN [dbMSConfig_OMS2K].MSStage10Desc 
			WHEN 11 THEN [dbMSConfig_OMS2K].MSStage11Desc 
			WHEN 12 THEN [dbMSConfig_OMS2K].MSStage12Desc 
			WHEN 13 THEN [dbMSConfig_OMS2K].MSStage13Desc 
			WHEN 14 THEN [dbMSConfig_OMS2K].MSStage14Desc 
			WHEN 15 THEN [dbMSConfig_OMS2K].MSStage15Desc 
			WHEN 16 THEN [dbMSConfig_OMS2K].MSStage16Desc 
			WHEN 17 THEN [dbMSConfig_OMS2K].MSStage17Desc 
			WHEN 18 THEN [dbMSConfig_OMS2K].MSStage18Desc 
			WHEN 19 THEN [dbMSConfig_OMS2K].MSStage19Desc 
			WHEN 20 THEN [dbMSConfig_OMS2K].MSStage20Desc 
			WHEN 21 THEN [dbMSConfig_OMS2K].MSStage21Desc 
			WHEN 22 THEN [dbMSConfig_OMS2K].MSStage22Desc 
			WHEN 23 THEN [dbMSConfig_OMS2K].MSStage23Desc 
			WHEN 24 THEN [dbMSConfig_OMS2K].MSStage24Desc 
			WHEN 25 THEN [dbMSConfig_OMS2K].MSStage25Desc 
			WHEN 26 THEN [dbMSConfig_OMS2K].MSStage26Desc 
			WHEN 27 THEN [dbMSConfig_OMS2K].MSStage27Desc 
			WHEN 28 THEN [dbMSConfig_OMS2K].MSStage28Desc 
			WHEN 29 THEN [dbMSConfig_OMS2K].MSStage29Desc 
			WHEN 30 THEN [dbMSConfig_OMS2K].MSStage30Desc 
	END) AS MileStoneDescription
	, MAX(CASE TStage.TStage 
			WHEN 1 THEN [dbMSData_OMS2K].MSStage1Due 
			WHEN 2 THEN [dbMSData_OMS2K].MSStage2Due 
			WHEN 3 THEN [dbMSData_OMS2K].MSStage3Due 
			WHEN 4 THEN [dbMSData_OMS2K].MSStage4Due 
			WHEN 5 THEN [dbMSData_OMS2K].MSStage5Due 
			WHEN 6 THEN [dbMSData_OMS2K].MSStage6Due 
			WHEN 7 THEN [dbMSData_OMS2K].MSStage7Due 
			WHEN 8 THEN [dbMSData_OMS2K].MSStage8Due 
			WHEN 9 THEN [dbMSData_OMS2K].MSStage9Due 
			WHEN 10 THEN [dbMSData_OMS2K].MSStage10Due 
			WHEN 11 THEN [dbMSData_OMS2K].MSStage11Due 
			WHEN 12 THEN [dbMSData_OMS2K].MSStage12Due 
			WHEN 13 THEN [dbMSData_OMS2K].MSStage13Due 
			WHEN 14 THEN [dbMSData_OMS2K].MSStage14Due 
			WHEN 15 THEN [dbMSData_OMS2K].MSStage15Due 
			WHEN 16 THEN [dbMSData_OMS2K].MSStage16Due 
			WHEN 17 THEN [dbMSData_OMS2K].MSStage17Due 
			WHEN 18 THEN [dbMSData_OMS2K].MSStage18Due 
			WHEN 19 THEN [dbMSData_OMS2K].MSStage19Due 
			WHEN 20 THEN [dbMSData_OMS2K].MSStage20Due 
			WHEN 21 THEN [dbMSData_OMS2K].MSStage21Due 
			WHEN 22 THEN [dbMSData_OMS2K].MSStage22Due 
			WHEN 23 THEN [dbMSData_OMS2K].MSStage23Due 
			WHEN 24 THEN [dbMSData_OMS2K].MSStage24Due 
			WHEN 25 THEN [dbMSData_OMS2K].MSStage25Due 
			WHEN 26 THEN [dbMSData_OMS2K].MSStage26Due 
			WHEN 27 THEN [dbMSData_OMS2K].MSStage27Due 
			WHEN 28 THEN [dbMSData_OMS2K].MSStage28Due 
			WHEN 29 THEN [dbMSData_OMS2K].MSStage29Due 
			WHEN 30 THEN [dbMSData_OMS2K].MSStage30Due 
	END) AS MilestoneDue
	, MAX(CASE TStage.TStage 
			WHEN 1 THEN [dbMSData_OMS2K].MSStage1Achieved 
			WHEN 2 THEN [dbMSData_OMS2K].MSStage2Achieved 
			WHEN 3 THEN [dbMSData_OMS2K].MSStage3Achieved 
			WHEN 4 THEN [dbMSData_OMS2K].MSStage4Achieved 
			WHEN 5 THEN [dbMSData_OMS2K].MSStage5Achieved 
			WHEN 6 THEN [dbMSData_OMS2K].MSStage6Achieved 
			WHEN 7 THEN [dbMSData_OMS2K].MSStage7Achieved 
			WHEN 8 THEN [dbMSData_OMS2K].MSStage8Achieved 
			WHEN 9 THEN [dbMSData_OMS2K].MSStage9Achieved 
			WHEN 10 THEN [dbMSData_OMS2K].MSStage10Achieved 
			WHEN 11 THEN [dbMSData_OMS2K].MSStage11Achieved 
			WHEN 12 THEN [dbMSData_OMS2K].MSStage12Achieved 
			WHEN 13 THEN [dbMSData_OMS2K].MSStage13Achieved 
			WHEN 14 THEN [dbMSData_OMS2K].MSStage14Achieved 
			WHEN 15 THEN [dbMSData_OMS2K].MSStage15Achieved 
			WHEN 16 THEN [dbMSData_OMS2K].MSStage16Achieved 
			WHEN 17 THEN [dbMSData_OMS2K].MSStage17Achieved 
			WHEN 18 THEN [dbMSData_OMS2K].MSStage18Achieved 
			WHEN 19 THEN [dbMSData_OMS2K].MSStage19Achieved 
			WHEN 20 THEN [dbMSData_OMS2K].MSStage20Achieved 
			WHEN 21 THEN [dbMSData_OMS2K].MSStage21Achieved 
			WHEN 22 THEN [dbMSData_OMS2K].MSStage22Achieved 
			WHEN 23 THEN [dbMSData_OMS2K].MSStage23Achieved 
			WHEN 24 THEN [dbMSData_OMS2K].MSStage24Achieved 
			WHEN 25 THEN [dbMSData_OMS2K].MSStage25Achieved 
			WHEN 26 THEN [dbMSData_OMS2K].MSStage26Achieved 
			WHEN 27 THEN [dbMSData_OMS2K].MSStage27Achieved 
			WHEN 28 THEN [dbMSData_OMS2K].MSStage28Achieved 
			WHEN 29 THEN [dbMSData_OMS2K].MSStage29Achieved 
			WHEN 30 THEN [dbMSData_OMS2K].MSStage30Achieved 
	END) AS MilestoneAchieved
	, MAX(CASE TStage.TStage 
			WHEN 1 THEN [dbMSData_OMS2K].MSStage1AchievedBy 
			WHEN 2 THEN [dbMSData_OMS2K].MSStage2AchievedBy 
			WHEN 3 THEN [dbMSData_OMS2K].MSStage3AchievedBy 
			WHEN 4 THEN [dbMSData_OMS2K].MSStage4AchievedBy 
			WHEN 5 THEN [dbMSData_OMS2K].MSStage5AchievedBy 
			WHEN 6 THEN [dbMSData_OMS2K].MSStage6AchievedBy 
			WHEN 7 THEN [dbMSData_OMS2K].MSStage7AchievedBy 
			WHEN 8 THEN [dbMSData_OMS2K].MSStage8AchievedBy 
			WHEN 9 THEN [dbMSData_OMS2K].MSStage9AchievedBy 
			WHEN 10 THEN [dbMSData_OMS2K].MSStage10AchievedBy 
			WHEN 11 THEN [dbMSData_OMS2K].MSStage11AchievedBy 
			WHEN 12 THEN [dbMSData_OMS2K].MSStage12AchievedBy 
			WHEN 13 THEN [dbMSData_OMS2K].MSStage13AchievedBy 
			WHEN 14 THEN [dbMSData_OMS2K].MSStage14AchievedBy 
			WHEN 15 THEN [dbMSData_OMS2K].MSStage15AchievedBy 
			WHEN 16 THEN [dbMSData_OMS2K].MSStage16AchievedBy 
			WHEN 17 THEN [dbMSData_OMS2K].MSStage17AchievedBy 
			WHEN 18 THEN [dbMSData_OMS2K].MSStage18AchievedBy 
			WHEN 19 THEN [dbMSData_OMS2K].MSStage19AchievedBy 
			WHEN 20 THEN [dbMSData_OMS2K].MSStage20AchievedBy 
			WHEN 21 THEN [dbMSData_OMS2K].MSStage21AchievedBy 
			WHEN 22 THEN [dbMSData_OMS2K].MSStage22AchievedBy 
			WHEN 23 THEN [dbMSData_OMS2K].MSStage23AchievedBy 
			WHEN 24 THEN [dbMSData_OMS2K].MSStage24AchievedBy 
			WHEN 25 THEN [dbMSData_OMS2K].MSStage25AchievedBy 
			WHEN 26 THEN [dbMSData_OMS2K].MSStage26AchievedBy 
			WHEN 27 THEN [dbMSData_OMS2K].MSStage27AchievedBy 
			WHEN 28 THEN [dbMSData_OMS2K].MSStage28AchievedBy 
			WHEN 29 THEN [dbMSData_OMS2K].MSStage29AchievedBy 
			WHEN 30 THEN [dbMSData_OMS2K].MSStage30AchievedBy 
	END) AS MilestoneAchievedBy
	, MAX(CASE TStage.TStage 
			WHEN 1 THEN [dbMSConfig_OMS2K].MSStage1LongDesc
			WHEN 2 THEN [dbMSConfig_OMS2K].MSStage2LongDesc
			WHEN 3 THEN [dbMSConfig_OMS2K].MSStage3LongDesc
			WHEN 4 THEN [dbMSConfig_OMS2K].MSStage4LongDesc
			WHEN 5 THEN [dbMSConfig_OMS2K].MSStage5LongDesc
			WHEN 6 THEN [dbMSConfig_OMS2K].MSStage6LongDesc
			WHEN 7 THEN [dbMSConfig_OMS2K].MSStage7LongDesc
			WHEN 8 THEN [dbMSConfig_OMS2K].MSStage8LongDesc
			WHEN 9 THEN [dbMSConfig_OMS2K].MSStage9LongDesc
			WHEN 10 THEN [dbMSConfig_OMS2K].MSStage10LongDesc
			WHEN 11 THEN [dbMSConfig_OMS2K].MSStage11LongDesc
			WHEN 12 THEN [dbMSConfig_OMS2K].MSStage12LongDesc
			WHEN 13 THEN [dbMSConfig_OMS2K].MSStage13LongDesc
			WHEN 14 THEN [dbMSConfig_OMS2K].MSStage14LongDesc
			WHEN 15 THEN [dbMSConfig_OMS2K].MSStage15LongDesc
			WHEN 16 THEN [dbMSConfig_OMS2K].MSStage16LongDesc
			WHEN 17 THEN [dbMSConfig_OMS2K].MSStage17LongDesc
			WHEN 18 THEN [dbMSConfig_OMS2K].MSStage18LongDesc
			WHEN 19 THEN [dbMSConfig_OMS2K].MSStage19LongDesc
			WHEN 20 THEN [dbMSConfig_OMS2K].MSStage20LongDesc
			WHEN 21 THEN [dbMSConfig_OMS2K].MSStage21LongDesc
			WHEN 22 THEN [dbMSConfig_OMS2K].MSStage22LongDesc
			WHEN 23 THEN [dbMSConfig_OMS2K].MSStage23LongDesc
			WHEN 24 THEN [dbMSConfig_OMS2K].MSStage24LongDesc
			WHEN 25 THEN [dbMSConfig_OMS2K].MSStage25LongDesc
			WHEN 26 THEN [dbMSConfig_OMS2K].MSStage26LongDesc
			WHEN 27 THEN [dbMSConfig_OMS2K].MSStage27LongDesc
			WHEN 28 THEN [dbMSConfig_OMS2K].MSStage28LongDesc
			WHEN 29 THEN [dbMSConfig_OMS2K].MSStage29LongDesc
			WHEN 30 THEN [dbMSConfig_OMS2K].MSStage30LongDesc
	END) AS MilestoneLongDesc
FROM 
	[dbo].[dbMSConfig_OMS2K]
INNER JOIN	
	[dbo].[dbMSData_OMS2K] ON [dbo].[dbMSData_OMS2K].MSCode = [dbo].[dbMSConfig_OMS2K].MSCode
INNER JOIN 
	[dbo].dbFile f ON f.fileID = [dbo].[dbMSData_OMS2K].[fileID] -- Added to secure a milestone to a dbFile so it cannot be accessed if not granted access to the dbfile
CROSS JOIN ( VALUES (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12), (13), (14), (15), (16), (17), (18), (19), (20), (21), (22), (23), (24), (25), (26), (27), (28), (29), (30) ) TStage (TStage)
WHERE [dbo].[dbMSData_OMS2K].[fileID] = @MATTER_ID
GROUP BY [dbMSData_OMS2K].[fileID], TStage.TStage
)
, [Data] AS (
SELECT DISTINCT [OMS2K].[fileID]
	, [OMS2K].TStage AS TaskStage
	, CAST([OMS2K].MileStoneDescription AS NVARCHAR(MAX)) AS MileStoneDescription
	, [OMS2K].MilestoneDue
	, [OMS2K].MilestoneAchieved
	, [OMS2K].MilestoneAchievedBy
	, [OMS2K].MilestoneLongDesc
	, @IsMileStone AS MileStoneOrTask
FROM OMS2K
	INNER JOIN [dbo].[dbTasks] ON [dbTasks].fileID = [OMS2K].[fileID] AND [dbTasks].tskMSStage = OMS2K.TStage
UNION ALL
SELECT [OMS2K].[fileID]
	, [OMS2K].TStage
	, dbTasks.tskDesc
	, dbTasks.tskDue
	, dbTasks.tskCompleted
	, dbTasks.tskCompletedBy
	, [OMS2K].MilestoneLongDesc
	, @IsTask AS MileStoneOrTask
FROM OMS2K
	INNER JOIN [dbo].[dbTasks] ON [dbTasks].fileID = [OMS2K].[fileID] AND [dbTasks].tskMSStage = OMS2K.TStage

)
SELECT  dt.MilestoneDescription
	, dt.MilestoneDue
	, dt.MilestoneAchieved
	, dbUser.usrFullName as AchievedBy
	, dt.MilestoneLongDesc
	, dt.TaskStage
	, dt.MileStoneOrTask
FROM [Data]AS dt
	LEFT OUTER JOIN dbUser ON dbUser.usrID = dt.MilestoneAchievedBy

END

SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMilestoneTasks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MatterMilestoneTasks] TO [OMSAdminRole]
    AS [dbo];

