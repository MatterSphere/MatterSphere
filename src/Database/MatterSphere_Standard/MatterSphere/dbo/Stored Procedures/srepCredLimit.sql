

CREATE PROCEDURE [dbo].[srepCredLimit]
(
	@UI uUICultureInfo='{default}',
	@FILETYPE nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@BRANCH nvarchar(50)= null,
	@STATUS nvarchar(50) = null,
	@FUNDTYPE nvarchar(50) = null,
	@LACAT nvarchar(50) = null,
	@FEEEARNER int = null
)

AS 

declare @Select nvarchar(3000)
declare @Where nvarchar(1000)
declare @orderby nvarchar(500)

--- BUILD THE SELECT CLAUSE
SET @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CL.clNo + ''/'' + F.fileNo + ''-'' + U.usrInits as FileRef,
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.fileCreditLimit,
	TL.SumTimeCharge, 
	CASE 
		WHEN FL.GrossValue IS NULL THEN 0
		ELSE FL.GrossValue
	END as GrossValue,
	Convert( decimal (10,2),
			(TL.SumTimeCharge + CASE WHEN FL.GrossValue IS NULL THEN 0 ELSE FL.GrossValue END)) as TimeDisbs,
	CASE
		WHEN F.fileCreditLimit = 0 THEN 0
		ELSE Convert( decimal(10,2), (TL.SumTimeCharge * 100) / F.fileCreditLimit)
	END as [Percentage Time],
	CASE
		WHEN F.fileCreditLimit = 0 THEN 0
		ELSE Convert( decimal(10,2) ,
			 ( (TL.SumTimeCharge) + (CASE WHEN FL.GrossValue IS NULL THEN 0 ELSE FL.GrossValue END) )
			 / F.fileCreditLimit * 100)
	END as [Percentage Time Disbs]
FROM
(
	SELECT
		T.fileID,
		CASE
			WHEN SUM(T.timeCharge) IS NULL THEN 0
			ELSE Convert ( decimal(10,2) , Sum(T.timeCharge))
		END as SumTimeCharge
	FROM
		dbo.dbTimeLedger T
	GROUP BY
		T.fileID
) TL
INNER JOIN
	dbFile F ON TL.fileID = F.fileID
LEFT OUTER JOIN
(
	SELECT
		FLD.fileId,
		SUM(FLD.fingross) as GrossValue
    FROM          
		dbFinancialLedger FLD
	GROUP BY
		FLD.fileID
) FL ON F.fileID = FL.fileID
LEFT OUTER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
LEFT OUTER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID'


-- where clauses
SET @WHERE = ''

-- filetype
IF (@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = N' WHERE F.fileType = @FILETYPE'
END

-- Fee Earner
IF (@FEEEARNER IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.filePrincipleID = @FEEEARNER'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.filePrincipleID = @FEEEARNER'
	END
END

-- status
IF (@STATUS IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.fileStatus = @STATUS'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.fileStatus = @STATUS'
	END
END

-- department
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.fileDepartment = @DEPARTMENT'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.fileDepartment = @DEPARTMENT'
	END
END


-- branch
IF (@BRANCH IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.brId = @BRANCH'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.brID = @BRANCH'
	END
END

-- fundcode
IF (@FUNDTYPE IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.fileFundCode = @FUNDTYPE'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.fileFundCode = @FUNDTYPE'
	END
END

-- Legal Aid Cat
IF (@LACAT IS NOT NULL)
BEGIN
	IF (@WHERE = '')
	BEGIN
		SET @WHERE = N' WHERE F.fileLACategory = @LACAT'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + N' AND F.fileLACategory = @LACAT'
	END
END


-- order by 
SET @ORDERBY = N' ORDER BY 1,2'
		
declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby + ' option (maxdop 1)'

--- Debug Print
-- print @sql

exec sp_executesql @sql, N'
	@UI nvarchar(10),
	@FILETYPE nvarchar(50),
	@DEPARTMENT nvarchar(50),
	@BRANCH nvarchar(50),
	@STATUS nvarchar(50),
	@FUNDTYPE nvarchar(50),
	@LACAT nvarchar(50),
	@FEEEARNER int',
	@UI,
	@FILETYPE,
	@DEPARTMENT,
	@BRANCH,
	@STATUS,
	@FUNDTYPE,
	@LACAT,
	@FEEEARNER
	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCredLimit] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCredLimit] TO [OMSAdminRole]
    AS [dbo];

