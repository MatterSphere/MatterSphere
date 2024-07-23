

CREATE PROCEDURE [dbo].[srepTimeSpentOnFiles]
(
	@UI uUICultureInfo = '{default}'
	, @FILETYPE nvarchar(15) = NULL
	, @DEPARTMENT nvarchar(15) = NULL
	, @BRANCH int = NULL
	, @FUNDINGTYPE nvarchar(15) = NULL
	, @LACAT tinyint = NULL
	, @FEEEARNER int = NULL
	, @STATUS nvarchar(15) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @sql1 nvarchar(4000)
DECLARE @sql2 nvarchar(4000)
DECLARE @sqlWhere nvarchar(3000)
DECLARE @ORDERBY nvarchar(100)

--- BUILD THE SELECT CLAUSE
SET @sql1 = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CL.clNo + ''/'' + F.fileNo + ''-'' + U.usrInits AS [Reference]
	, CL.clName
	, REPLACE(F.fileDesc, char(13) + char(10), '', '') as [File Description]
	, X.cdDesc AS [Funding Type]
	, Y.cdDesc AS [Department]
	, TL.[Time Charge] as [Time Charge]
	, Convert (decimal(10,2), (TL.[Dtime Hours] + TL.[Dtime Mins]) ) as [DtlHoursMins]
	, Convert (decimal(10,2), (TM.[Gtime Hours] + TM.[Gtime Mins]) ) as [GlbHoursMins]
	, TM.[GTotal Charge]
FROM
(
	SELECT
		Convert(decimal(10,2), Sum(T.timeCharge)) as [GTotal Charge]
		, CASE
			WHEN SUM(T.timeMins) = 0 THEN 0.0
			ELSE CAST(SUM(T.timeMins) / 60 as DECIMAL(10,2))
		  END AS [GTime Hours]
		, CASE
			WHEN SUM(T.timeMins) = 0 THEN 0.0
			ELSE CAST(SUM(T.timeMins) % 60 as DECIMAL (10,2)) / 100
		  END AS [GTime Mins]
	FROM
		dbTimeLedger T
	INNER JOIN
		dbFile F ON T.fileID = F.fileID
	WHERE
		(T.timeRecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE) xxx'


	SET @sqlWhere = ''

	--- FILE TYPE CLAUSE
	IF @FILETYPE IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.fileType = @FILETYPE'

	--- DEPARTMENT CLAUSE
	IF @DEPARTMENT IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.fileDepartment = @DEPARTMENT'

	--- BRANCH CLAUSE
	IF @BRANCH IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.brID = @BRANCH'

	--- FUNDING TYPE CLAUSE
	IF @FUNDINGTYPE IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.fileFundCode = @FUNDINGTYPE'

	--- LEGAL AID CATEGORY CLAUSE
	IF @LACAT IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.fileLACategory = @LACAT'

	--- FILE STATUS CLAUSE
	IF @STATUS IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.fileStatus = @STATUS'

	--- FEE EARNER CLAUSE
	IF @FEEEARNER IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND F.filePrincipleID = @FEEEARNER'

	SET @sql1 = REPLACE ( @sql1 , 'xxx' , @sqlWhere )
	SET @sql1 = @sql1 +  ' ) TM '

SET @sql2 = N'
CROSS JOIN
(
	Select
		T.fileId
		, SUM(T.timeCharge) AS [Time Charge]
		, CASE
			WHEN SUM(T.timeMins) = 0 THEN 0.0
			ELSE CAST(SUM(T.timeMins) / 60 as DECIMAL(10,2))
		  END AS [DTime Hours]
		, CASE
			WHEN SUM(T.timeMins) = 0 THEN 0.0
			ELSE CAST(SUM(T.timeMins) % 60 as DECIMAL (10,2)) / 100
		  END AS [DTime Mins]
	FROM
		dbTimeLedger T
	WHERE
		(T.timeRecorded >= @STARTDATE AND T.timeRecorded < @ENDDATE)
	GROUP BY
		T.fileID
) TL
INNER JOIN
	dbFile F ON TL.fileID = F.fileID
INNER JOIN        
	dbo.dbClient CL ON F.clId = CL.clId
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''FUNDTYPE'', @UI) AS X ON X.cdCode = F.fileFundCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''DEPT'', @UI) AS Y ON Y.cdCode = F.fileDepartment'

---- SET THE WHERE CLAUSE
SET @sqlWhere = ''

--- FILE TYPE CLAUSE
IF @FILETYPE IS NOT NULL
	SET @sqlWhere = ' WHERE F.fileType = @FILETYPE'

--- DEPARTMENT CLAUSE
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.fileDepartment = @DEPARTMENT'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.fileDepartment = @DEPARTMENT'
	END
END

--- BRANCH CLAUSE
IF (@BRANCH IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.brID = @BRANCH'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.brID = @BRANCH'
	END
END

--- FUNDING TYPE CLAUSE
IF (@FUNDINGTYPE IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.fileFundCode = @FUNDINGTYPE'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.fileFundCode = @FUNDINGTYPE'
	END
END

--- LEGAL AID CATEGORY CLAUSE
IF (@LACAT IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.fileLACategory = @LACAT'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.fileLACategory = @LACAT'
	END
END

--- FILE STATUS CLAUSE
IF (@STATUS IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.fileStatus = @STATUS'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.fileStatus = @STATUS'
	END
END

--- FEE EARNER CLAUSE
IF (@FEEEARNER IS NOT NULL)
BEGIN
	IF (@sqlWhere = '')
	BEGIN
		SET @sqlWhere = ' WHERE F.filePrincipleID = @FEEEARNER'
	END
	ELSE
	BEGIN
		SET @sqlWhere = @sqlWhere + ' AND F.filePrincipleID = @FEEEARNER'
	END
END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = ' ORDER BY 1,2'

DECLARE @SQL nvarchar(4000)
--- ADD CLAUSES TOGETHER
SET @SQL = @sql1 + @sql2 + @sqlWhere + @ORDERBY + ' option (maxdop 1)'

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FILETYPE uCodeLookup
	, @DEPARTMENT uCodeLookup
	, @BRANCH int
	, @FUNDINGTYPE uCodeLookup
	, @LACAT tinyint
	, @FEEEARNER int
	, @STATUS uCodeLookup
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @FILETYPE
	, @DEPARTMENT
	, @BRANCH
	, @FUNDINGTYPE
	, @LACAT
	, @FEEEARNER
	, @STATUS
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeSpentOnFiles] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepTimeSpentOnFiles] TO [OMSAdminRole]
    AS [dbo];

