CREATE VIEW [dbo].[vwMilestones]
AS
--RN & PC 20180615: Modified for v8 30 milestones
SELECT TOP 100 PERCENT
	X.*,
	Y.MSStageDesc
 FROM 
(
	SELECT fileid , MSStage1AchievedBy as AchievedBy , MSCode , 1 as SubCode , MSStage1Due as StageDue , MSStage1Achieved as StageAchieved FROM dbMSData_OMS2K
	UNION ALL
	SELECT fileid , MSStage2AchievedBy as AchievedBy , MSCode , 2 as SubCode , MSStage2Due as StageDue , MSStage2Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage3AchievedBy as AchievedBy , MSCode , 3 as SubCode , MSStage3Due as StageDue , MSStage3Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage4AchievedBy as AchievedBy , MSCode , 4 as SubCode , MSStage4Due as StageDue , MSStage4Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage5AchievedBy as AchievedBy , MSCode , 5 as SubCode , MSStage5Due as StageDue , MSStage5Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage6AchievedBy as AchievedBy , MSCode , 6 as SubCode , MSStage6Due as StageDue , MSStage6Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage7AchievedBy as AchievedBy , MSCode , 7 as SubCode , MSStage7Due as StageDue , MSStage7Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage8AchievedBy as AchievedBy , MSCode , 8 as SubCode , MSStage8Due as StageDue , MSStage8Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL	
	SELECT fileid , MSStage9AchievedBy as AchievedBy , MSCode , 9 as SubCode , MSStage9Due as StageDue , MSStage9Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage10AchievedBy as AchievedBy , MSCode , 10 as SubCode , MSStage10Due as StageDue , MSStage10Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage11AchievedBy as AchievedBy , MSCode , 11 as SubCode , MSStage11Due as StageDue , MSStage11Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage12AchievedBy as AchievedBy , MSCode , 12 as SubCode , MSStage12Due as StageDue , MSStage12Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL	
	SELECT fileid , MSStage13AchievedBy as AchievedBy , MSCode , 13 as SubCode , MSStage13Due as StageDue , MSStage13Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage14AchievedBy as AchievedBy , MSCode , 14 as SubCode , MSStage14Due as StageDue , MSStage14Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage15AchievedBy as AchievedBy , MSCode , 15 as SubCode , MSStage15Due as StageDue , MSStage15Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage16AchievedBy as AchievedBy , MSCode , 16 as SubCode , MSStage16Due as StageDue , MSStage16Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL	
	SELECT fileid , MSStage17AchievedBy as AchievedBy , MSCode , 17 as SubCode , MSStage17Due as StageDue , MSStage17Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage18AchievedBy as AchievedBy , MSCode , 18 as SubCode , MSStage18Due as StageDue , MSStage18Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage19AchievedBy as AchievedBy , MSCode , 19 as SubCode , MSStage19Due as StageDue , MSStage19Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage20AchievedBy as AchievedBy , MSCode , 20 as SubCode , MSStage20Due as StageDue , MSStage20Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage21AchievedBy as AchievedBy , MSCode , 21 as SubCode , MSStage21Due as StageDue , MSStage21Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage22AchievedBy as AchievedBy , MSCode , 22 as SubCode , MSStage22Due as StageDue , MSStage22Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage23AchievedBy as AchievedBy , MSCode , 23 as SubCode , MSStage23Due as StageDue , MSStage23Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL	
	SELECT fileid , MSStage24AchievedBy as AchievedBy , MSCode , 24 as SubCode , MSStage24Due as StageDue , MSStage24Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage25AchievedBy as AchievedBy , MSCode , 25 as SubCode , MSStage25Due as StageDue , MSStage25Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage26AchievedBy as AchievedBy , MSCode , 26 as SubCode , MSStage26Due as StageDue , MSStage26Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage27AchievedBy as AchievedBy , MSCode , 27 as SubCode , MSStage27Due as StageDue , MSStage27Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL	
	SELECT fileid , MSStage28AchievedBy as AchievedBy , MSCode , 28 as SubCode , MSStage28Due as StageDue , MSStage28Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage29AchievedBy as AchievedBy , MSCode , 29 as SubCode , MSStage29Due as StageDue , MSStage29Achieved as StageAchieved FROM dbMSData_OMS2k
	UNION ALL
	SELECT fileid , MSStage30AchievedBy as AchievedBy , MSCode , 30 as SubCode , MSStage30Due as StageDue , MSStage30Achieved as StageAchieved FROM dbMSData_OMS2k
) X 
JOIN
(
	SELECT MSCode , MSStage1Desc as MSStageDesc , 1 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage2Desc as MSStageDesc , 2 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage3Desc as MSStageDesc , 3 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage4Desc as MSStageDesc , 4 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage5Desc as MSStageDesc , 5 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage6Desc as MSStageDesc , 6 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage7Desc as MSStageDesc , 7 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage8Desc as MSStageDesc , 8 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage9Desc as MSStageDesc , 9 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage10Desc as MSStageDesc , 10 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage11Desc as MSStageDesc , 11 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage12Desc as MSStageDesc , 12 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage13Desc as MSStageDesc , 13 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage14Desc as MSStageDesc , 14 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage15Desc as MSStageDesc , 15 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage16Desc as MSStageDesc , 16 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage17Desc as MSStageDesc , 17 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage18Desc as MSStageDesc , 18 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage19Desc as MSStageDesc , 19 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage20Desc as MSStageDesc , 20 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage21Desc as MSStageDesc , 21 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage22Desc as MSStageDesc , 22 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage23Desc as MSStageDesc , 23 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage24Desc as MSStageDesc , 24 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage25Desc as MSStageDesc , 25 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage26Desc as MSStageDesc , 26 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage27Desc as MSStageDesc , 27 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage28Desc as MSStageDesc , 28 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage29Desc as MSStageDesc , 29 as SubCode FROM dbo.dbMSConfig_OMS2K
	UNION ALL
	SELECT MSCode , MSStage30Desc as MSStageDesc , 30 as SubCode FROM dbo.dbMSConfig_OMS2K
) Y ON X.MSCode = Y.MSCode AND X.SubCode = Y.SubCode

ORDER BY
	X.fileID ,
	X.SubCode


GO