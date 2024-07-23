
CREATE PROCEDURE [dbo].[srepMilestoneMatterPlan] 
(
	@clno nvarchar(12),
	@fileno nvarchar(20),
	@STARTDATE datetime
)

--RN & PC 20180615: Modified for v8 30 milestones

AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	MSD.MSCode,
	MSD.MSNextDueDate,
	MSD.MSNextDueStage, 
	MSC.MSDescription,
	MSC.MSStage1Desc,
	MSC.MSStage2Desc, 
	MSC.MSStage3Desc,
	MSC.MSStage4Desc,
	MSC.MSStage5Desc, 
	MSC.MSStage6Desc,
	MSC.MSStage7Desc,
	MSC.MSStage8Desc, 
	MSC.MSStage9Desc,
	MSC.MSStage10Desc,
	MSC.MSStage11Desc, 
	MSC.MSStage12Desc,
	MSC.MSStage13Desc,
	MSC.MSStage14Desc, 
	MSC.MSStage15Desc,
	MSC.MSStage16Desc,
	MSC.MSStage17Desc, 
	MSC.MSStage18Desc,
	MSC.MSStage19Desc,
	MSC.MSStage20Desc, 
	MSC.MSStage21Desc, 
	MSC.MSStage22Desc, 
	MSC.MSStage23Desc, 
	MSC.MSStage24Desc, 
	MSC.MSStage25Desc, 
	MSC.MSStage26Desc, 
	MSC.MSStage27Desc, 
	MSC.MSStage28Desc, 
	MSC.MSStage29Desc, 
	MSC.MSStage30Desc, 
	MSD.MSStage1Due,
	MSD.MSStage2Due,
	MSD.MSStage3Due, 
	MSD.MSStage4Due,
	MSD.MSStage5Due,
	MSD.MSStage6Due, 
	MSD.MSStage7Due,
	MSD.MSStage8Due,
	MSD.MSStage9Due, 
	MSD.MSStage10Due,
	MSD.MSStage11Due,
	MSD.MSStage12Due, 
	MSD.MSStage13Due,
	MSD.MSStage14Due,
	MSD.MSStage15Due, 
	MSD.MSStage16Due,
	MSD.MSStage18Due,
	MSD.MSStage17Due, 
	MSD.MSStage19Due,
	MSD.MSStage20Due,
	MSD.MSStage21Due,
	MSD.MSStage22Due,
	MSD.MSStage23Due,
	MSD.MSStage24Due,
	MSD.MSStage25Due,
	MSD.MSStage26Due,
	MSD.MSStage27Due,
	MSD.MSStage28Due,
	MSD.MSStage29Due,
	MSD.MSStage30Due,
	MSD.MSStage1Achieved, 
	MSD.MSStage2Achieved,
	MSD.MSStage3Achieved,
	MSD.MSStage4Achieved, 
	MSD.MSStage5Achieved,
	MSD.MSStage6Achieved,
	MSD.MSStage7Achieved, 
	MSD.MSStage8Achieved,
	MSD.MSStage9Achieved,
	MSD.MSStage10Achieved, 
	MSD.MSStage11Achieved,
	MSD.MSStage12Achieved,
	MSD.MSStage13Achieved, 
	MSD.MSStage14Achieved,
	MSD.MSStage15Achieved,
	MSD.MSStage16Achieved, 
	MSD.MSStage17Achieved,
	MSD.MSStage18Achieved,
	MSD.MSStage19Achieved, 
	MSD.MSStage20Achieved,
	MSD.MSStage21Achieved,
	MSD.MSStage22Achieved,
	MSD.MSStage23Achieved,
	MSD.MSStage24Achieved,
	MSD.MSStage25Achieved,
	MSD.MSStage26Achieved,
	MSD.MSStage27Achieved,
	MSD.MSStage28Achieved,
	MSD.MSStage29Achieved,
	MSD.MSStage30Achieved,
	CASE
		WHEN MSD.MSStage1Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage1Due IS NOT NULL AND MSD.MSStage1Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage1Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage1Due IS NOT NULL AND MSD.MSStage1Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage1Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus1,
	CASE
		WHEN MSD.MSStage2Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage2Due IS NOT NULL AND MSD.MSStage2Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage2Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage2Due IS NOT NULL AND MSD.MSStage2Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage2Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus2,
	CASE
		WHEN MSD.MSStage3Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage3Due IS NOT NULL AND MSD.MSStage3Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage3Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage3Due IS NOT NULL AND MSD.MSStage3Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage3Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus3,
	CASE
		WHEN MSD.MSStage4Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage4Due IS NOT NULL AND MSD.MSStage4Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage4Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage4Due IS NOT NULL AND MSD.MSStage4Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage4Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus4,
	CASE
		WHEN MSD.MSStage5Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage5Due IS NOT NULL AND MSD.MSStage5Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage5Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage5Due IS NOT NULL AND MSD.MSStage5Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage5Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus5,
	CASE
		WHEN MSD.MSStage6Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage6Due IS NOT NULL AND MSD.MSStage6Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage6Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage6Due IS NOT NULL AND MSD.MSStage6Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage6Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus6,
	CASE
		WHEN MSD.MSStage7Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage7Due IS NOT NULL AND MSD.MSStage7Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage7Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage7Due IS NOT NULL AND MSD.MSStage7Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage7Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus7,
	CASE
		WHEN MSD.MSStage8Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage8Due IS NOT NULL AND MSD.MSStage8Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage8Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage8Due IS NOT NULL AND MSD.MSStage8Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage8Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus8,
	CASE
		WHEN MSD.MSStage9Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage9Due IS NOT NULL AND MSD.MSStage9Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage9Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage9Due IS NOT NULL AND MSD.MSStage9Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage9Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus9,
	CASE
		WHEN MSD.MSStage10Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage10Due IS NOT NULL AND MSD.MSStage10Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage10Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage10Due IS NOT NULL AND MSD.MSStage10Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage10Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus10,
	CASE
		WHEN MSD.MSStage11Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage11Due IS NOT NULL AND MSD.MSStage11Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage11Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage11Due IS NOT NULL AND MSD.MSStage11Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage11Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus11,
	CASE
		WHEN MSD.MSStage12Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage12Due IS NOT NULL AND MSD.MSStage12Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage12Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage12Due IS NOT NULL AND MSD.MSStage12Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage12Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus12,
	CASE
		WHEN MSD.MSStage13Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage13Due IS NOT NULL AND MSD.MSStage13Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage13Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage13Due IS NOT NULL AND MSD.MSStage13Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage13Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus13,
	CASE
		WHEN MSD.MSStage14Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage14Due IS NOT NULL AND MSD.MSStage14Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage14Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage14Due IS NOT NULL AND MSD.MSStage14Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage14Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus14,
	CASE
		WHEN MSD.MSStage15Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage15Due IS NOT NULL AND MSD.MSStage15Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage15Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage15Due IS NOT NULL AND MSD.MSStage15Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage15Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus15,
	CASE
		WHEN MSD.MSStage16Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage16Due IS NOT NULL AND MSD.MSStage16Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage16Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage16Due IS NOT NULL AND MSD.MSStage16Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage16Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus16,
	CASE
		WHEN MSD.MSStage17Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage17Due IS NOT NULL AND MSD.MSStage17Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage17Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage17Due IS NOT NULL AND MSD.MSStage17Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage17Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus17,
	CASE
		WHEN MSD.MSStage18Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage18Due IS NOT NULL AND MSD.MSStage18Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage18Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage18Due IS NOT NULL AND MSD.MSStage18Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage18Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus18,
	CASE
		WHEN MSD.MSStage19Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage19Due IS NOT NULL AND MSD.MSStage19Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage19Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage19Due IS NOT NULL AND MSD.MSStage19Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage19Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus19,
	CASE
		WHEN MSD.MSStage20Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage20Due IS NOT NULL AND MSD.MSStage20Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage20Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage20Due IS NOT NULL AND MSD.MSStage20Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage20Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus20,
	CASE
		WHEN MSD.MSStage21Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage21Due IS NOT NULL AND MSD.MSStage21Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage21Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage21Due IS NOT NULL AND MSD.MSStage21Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage21Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus21,
	CASE
		WHEN MSD.MSStage22Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage22Due IS NOT NULL AND MSD.MSStage22Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage22Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage22Due IS NOT NULL AND MSD.MSStage22Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage22Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus22,
	CASE
		WHEN MSD.MSStage23Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage23Due IS NOT NULL AND MSD.MSStage23Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage23Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage23Due IS NOT NULL AND MSD.MSStage23Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage23Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus23,
	CASE
		WHEN MSD.MSStage24Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage24Due IS NOT NULL AND MSD.MSStage24Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage24Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage24Due IS NOT NULL AND MSD.MSStage24Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage24Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus24,
	CASE
		WHEN MSD.MSStage25Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage25Due IS NOT NULL AND MSD.MSStage25Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage25Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage25Due IS NOT NULL AND MSD.MSStage25Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage25Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus25,
	CASE
		WHEN MSD.MSStage26Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage26Due IS NOT NULL AND MSD.MSStage26Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage26Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage26Due IS NOT NULL AND MSD.MSStage26Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage26Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus26,
	CASE
		WHEN MSD.MSStage27Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage27Due IS NOT NULL AND MSD.MSStage27Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage27Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage27Due IS NOT NULL AND MSD.MSStage27Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage27Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus27,
	CASE
		WHEN MSD.MSStage28Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage28Due IS NOT NULL AND MSD.MSStage28Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage28Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage28Due IS NOT NULL AND MSD.MSStage28Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage28Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus28,
	CASE
		WHEN MSD.MSStage29Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage29Due IS NOT NULL AND MSD.MSStage29Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage29Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage29Due IS NOT NULL AND MSD.MSStage29Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage29Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus29,
	CASE
		WHEN MSD.MSStage30Achieved IS NOT NULL THEN ' Completed Milestone'
		WHEN MSD.MSStage30Due IS NOT NULL AND MSD.MSStage30Due >= @STARTDATE THEN
			 CAST(DateDiff(dd, @STARTDATE, MSD.MSStage30Due) AS varchar(4)) + ' day(s) to complete'
		WHEN MSD.MSStage30Due IS NOT NULL AND MSD.MSStage30Due < @STARTDATE THEN
			 CAST(DateDiff(dd, MSD.MSStage30Due, @STARTDATE) AS varchar(4)) + ' day(s) overdue'
		ELSE ''
	END AS StageStatus30,
	CL.clNo,
	F.fileNo,
	CL.clName,
	F.Created,
	F.fileDesc,
	F.filePrincipleID, 
	U.usrInits AS FeeInits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage1achievedby) AS Achieved1Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage2achievedby) AS Achieved2Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage3achievedby) AS Achieved3Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage4achievedby) AS Achieved4Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage5achievedby) AS Achieved5Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage6achievedby) AS Achieved6Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage7achievedby) AS Achieved7Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage8achievedby) AS Achieved8Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage9achievedby) AS Achieved9Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage10achievedby) AS Achieved10Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage11achievedby) AS Achieved11Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage12achievedby) AS Achieved12Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage13achievedby) AS Achieved13Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage14achievedby) AS Achieved14Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage15achievedby) AS Achieved15Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage16achievedby) AS Achieved16Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage17achievedby) AS Achieved17Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage18achievedby) AS Achieved18Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage19achievedby) AS Achieved19Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage20achievedby) AS Achieved20Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage21achievedby) AS Achieved21Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage22achievedby) AS Achieved22Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage23achievedby) AS Achieved23Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage24achievedby) AS Achieved24Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage25achievedby) AS Achieved25Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage26achievedby) AS Achieved26Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage27achievedby) AS Achieved27Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage28achievedby) AS Achieved28Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage29achievedby) AS Achieved29Inits,
	(SELECT usrinits FROM dbuser WHERE usrid = msstage30achievedby) AS Achieved30Inits
FROM
	dbo.dbMSConfig_OMS2K MSC
INNER JOIN
	dbo.dbMSData_OMS2K MSD ON MSC.MSCode = MSD.MSCode
INNER JOIN
	dbo.dbFile F ON MSD.fileID = F.fileID
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
WHERE
	(CL.clNo = @clno) AND (F.fileNo = @fileno) 

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[srepMilestoneMatterPlan] TO [OMSRole]
	AS [dbo];


GO
GRANT EXECUTE
	ON OBJECT::[dbo].[srepMilestoneMatterPlan] TO [OMSAdminRole]
	AS [dbo];

