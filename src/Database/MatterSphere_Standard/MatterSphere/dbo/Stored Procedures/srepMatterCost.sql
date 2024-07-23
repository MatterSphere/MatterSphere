

CREATE PROCEDURE [dbo].[srepMatterCost]
(
	@UI uUICultureInfo = '{default}'
	, @STARTDATE datetime = NULL
	, @LACAT nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(1000)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo
	, F.fileNo 
	, CL.clName 
	, replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc
	, U.usrInits
	, F.Created
	, Y.finGross
	, BI.billProCosts
	, LACAT.LegAidDesc
	, FT.ftLegalAidCharged
	, X.Wip AS Wip
	, CASE
		WHEN Y.finGross IS NULL THEN 0
		ELSE Y.finGross
	  END +
	  CASE
		WHEN BI.billProCosts IS NULL THEN 0
		ELSE BI.billProCosts
	  END +
	  CASE
		WHEN X.Wip IS NULL THEN 0
		ELSE X.Wip
	  END AS CostIncWip
FROM         
	dbo.dbLegalAidCategory LACAT
INNER JOIN
	dbo.dbFile F ON LACAT.LegAIdCategory = F.fileLACategory 
INNER JOIN
	dbo.dbClient CL ON F.clId = CL.clID
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
	dbo.dbFundType FT ON F.fileFundCode = FT.ftCode AND F.filecurISOCode = FT.ftcurISOCode
LEFT OUTER JOIN
	dbo.dbBillInfo BI ON F.fileID = BI.fileId
LEFT OUTER JOIN
	(SELECT
		FL.fileID,
		Sum(FL.finGross) AS finGross
	 FROM
		dbFinancialLedger FL
	 GROUP BY
		FL.fileId
	 ) AS Y ON Y.fileID = F.fileID 
LEFT OUTER JOIN
	(SELECT
		T.fileID,
		Sum(T.timeCharge) AS Wip
	 FROM
		dbTimeLedger T
	 WHERE
		T.timeBilled = 0
	 GROUP BY
		T.fileId
	 ) AS X ON X.fileID = BI.fileID '

---- SET THE WHERE STATEMENT
--SET @WHERE = ' FT.ftLegalAidCharged = ''1'' AND CL.clNo = ''M65640'' '
SET @WHERE = ' FT.ftLegalAidCharged = ''1'' '

--- LEGAL AID CATEGORY CLAUSE
IF(@LACAT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND LACAT.LegAIdCategory = @LACAT '
END

--- FILE CREATED CLAUSE
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.Created >= @STARTDATE '
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

-- DEBUG PRINT
 PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @STARTDATE datetime
	, @LACAT nvarchar(15) '
	, @UI
	, @STARTDATE
	, @LACAT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMatterCost] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMatterCost] TO [OMSAdminRole]
    AS [dbo];

