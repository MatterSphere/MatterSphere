

CREATE PROCEDURE [dbo].[srepFillLauKcMsr]
(
	@UI uUICultureInfo='{default}'
	, @CONTRACTNAME nvarchar(50) = NULL
	, @LACAT nvarchar(50) = null
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nVarChar(2000)
DECLARE @WHERE nVarChar(2000)
DECLARE @SQL nVarCHar(4000)

-- Start of the select, checking if the user has asked for units or values
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	LAC.LAContractName, 
	LAC.LAContractCode, 
	LAC.LAContractRef,
	X.cdDesc,
	F.Created, 
	FL.MatLAMatType, 
	CL.clNo, 
	F.fileNo, 
	F.filePrincipleID, 
    U.usrInits, 
	U.usrFullName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	CL.clName, 
	F.fileLACategory
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
INNER JOIN
	dbContact C ON C.contID = CL.clDefaultContact
INNER JOIN
	dbFileLegal FL ON FL.fileID = F.fileID
INNER JOIN
	dbo.dbLegalAidContract LAC ON LAC.LAContractCode = FL.MatLAContract
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''CONTTYPE'', @UI) AS X ON X.cdCode = C.contTypeCode '

-- where clause
SET @WHERE = N''

-- legal aid cat
IF (@LACAT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' f.fileLACategory = @LACAT '
END

-- contract name
IF (@CONTRACTNAME IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND LAC.LAContractCode = @CONTRACTNAME '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' LAC.LAContractCode = @CONTRACTNAME '
	END
END

-- Date ranges
IF (@STARTDATE IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (F.created >= @STARTDATE AND F.created < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (F.created >= @STARTDATE AND F.created < @ENDDATE) '
	END
END

-- finish of the where clause
IF (@WHERE <> '')
BEGIN
	SET @WHERE = ' WHERE ' + @WHERE
END

-- combine into 1 query
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @CONTRACTNAME nvarchar(50)
	, @LACAT nvarchar(50)
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @CONTRACTNAME
	, @LACAT
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFillLauKcMsr] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFillLauKcMsr] TO [OMSAdminRole]
    AS [dbo];

