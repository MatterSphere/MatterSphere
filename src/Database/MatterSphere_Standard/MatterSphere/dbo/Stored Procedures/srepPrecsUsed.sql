

CREATE PROCEDURE [dbo].[srepPrecsUsed]
(
	@UI uUICultureInfo='{default}',
	@FEEEARNER nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS 

DECLARE @SQL nvarchar(4000)
DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(1500)
DECLARE @GROUPBY nvarchar(1000)
DECLARE @ORDERBY nvarchar(500)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	U1.usrInits AS DocCreatedByInits, 
    U1.usrFullName AS DocCreatedByName,
	X.cdDesc AS Department,
	COUNT(P.precID) AS TotalPrecsUsed
FROM
	dbFile F
INNER JOIN         
	dbo.dbDocument DOC ON F.fileID = DOC.fileID
INNER JOIN
	dbo.dbPrecedents P ON CASE WHEN DOC.docprecID IS NULL THEN DOC.docBasePrecID ELSE DOC.docPrecID END = P.PrecID 
INNER JOIN
	dbo.dbUser U1 ON DOC.Createdby = U1.usrID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI ) X ON X.[cdCode] = F.[fileDepartment] '

-- Build the WHere clause
SET @WHERE = N''

-- Fee Earner Filter
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' F.filePrincipleID = @FEEEARNER '
END

-- Department Clause
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileDepartment = @DEPARTMENT '
	END
END

-- Startdate filter
IF (@STARTDATE IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (DOC.Created >= @STARTDATE AND DOC.Created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (DOC.Created >= @STARTDATE AND DOC.Created < @ENDDATE) '
	END
END

-- finish the where clause
IF (@WHERE <> '')
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

-- Group by clause
SET @GROUPBY = N'
GROUP BY
	U1.usrInits, 
    U1.usrFullName,
	X.cdDesc '

-- Order By Clause
SET @ORDERBY = N'
ORDER BY
	Department '


-- now build the whole thing
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@GROUPBY) + Rtrim(@ORDERBY)

--- Debug Print
--  PRINT @SQL

exec sp_executesql @sql, N'
	@UI nvarchar(10),
	@FEEEARNER nvarchar(50),
	@DEPARTMENT nvarchar(50),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@FEEEARNER,
	@DEPARTMENT,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecsUsed] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepPrecsUsed] TO [OMSAdminRole]
    AS [dbo];

