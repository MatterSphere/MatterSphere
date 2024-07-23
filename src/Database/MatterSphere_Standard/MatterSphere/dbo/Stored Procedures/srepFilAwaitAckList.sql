

CREATE PROCEDURE [dbo].[srepFilAwaitAckList]
(
	@UI uUICultureInfo='{default}',
	@FILETYPE uCodeLookup = null,
	@FEEEARNER BigInt = null,
	@DEPARTMENT uCodeLookup = null,
	@FUNDTYPE uCodeLookup = null,
	@BRANCH BigInt = null,
	@STARTDATE DateTime = null,
	@ENDDATE DateTime = null
)

AS 

DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(2000)

--- Select Statement for the base Query
set @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	F.fileAllowExternal,
	U.usrInits AS FeeInits, 
	U.usrFullName AS FeeName, 
	dbo.GetFileRef(CL.clNo, F.fileNo) AS OurRef,
	CL.clName,
	Replace(F.fileDesc, char(13) + char(10), '', '') AS fileDesc,
	A.cdDesc AS fileTypeDesc,
	A.cdDesc AS FundTypeDesc,
	A.cdDesc AS DeptDesc,
	F.fileextlinktxtid
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI ) A ON A.[cdCode] = F.[fileDepartment]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FILETYPE'' , @UI ) B ON B.[cdCode] = F.[fileType]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FUNDTYPE'' , @UI ) C ON C.[cdCode] = F.[fileFundCode]'

-- Build the where clause
SET @WHERE = N' WHERE F.fileStatus = ''LIVEAWAITACK'' '

-- Filetype filter
IF (@FILETYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileType = @FILETYPE '
END

-- FeeEarner
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND U.usrID = @FEEEARNER '
END

-- Department
IF (@DEPARTMENT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT '
END

-- Fund Type
IF (@FUNDTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.fileFundCode = @FUNDTYPE '
END

-- Branch
IF (@BRANCH IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND F.brID = @BRANCH '
END

-- Start Date
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE) '
END

-- Debug Print
-- PRINT @SQL

-- create the query
DECLARE @SQL nvarchar(4000)
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

exec sp_executesql @SQL, N'
	@UI nvarchar(10),
	@FILETYPE uCodeLookup,
	@FEEEARNER BigInt,
	@DEPARTMENT uCodeLookup,
	@FUNDTYPE uCodeLookup,
	@BRANCH BigInt,
	@STARTDATE DateTime,
	@ENDDATE DateTime',
	@UI,
	@FILETYPE,
	@FEEEARNER,
	@DEPARTMENT,	
	@FUNDTYPE,
	@BRANCH,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAwaitAckList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAwaitAckList] TO [OMSAdminRole]
    AS [dbo];

