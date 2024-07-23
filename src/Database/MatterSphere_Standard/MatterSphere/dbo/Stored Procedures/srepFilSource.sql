

CREATE PROCEDURE [dbo].[srepFilSource]
(
	@UI uUICultureInfo='{default}',
	@SOURCE nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@STARTDATE DateTime = null,
	@ENDDATE DateTime = null,
	@STATUS nvarchar(30) =null,
	@TYPE nvarchar(30) = null
)

AS 

DECLARE @SQL nvarchar(4000)
DECLARE @SELECT nvarchar(1500)
DECLARE @WHERE nvarchar(1000)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT  
	CL.clNo + ''/'' + F.fileNo AS OurRef,
	CL.clName,
	F.fileDesc,
	CASE
		WHEN F.fileSource IS NULL THEN ''(Not Set)''
		ELSE Replace(Y.[cdDesc],''%CLIENT%'',''Client'')
	END AS Source,
	X.[cdDesc] as Department,
	CASE
		WHEN F.fileSource = ''CONTACT'' THEN 1
		WHEN F.fileSource = ''USER'' THEN 2
		ELSE 3 
	END AS User_Contact,
	C.contName AS SourceCont,
	U1.usrFullName AS SourceUser,
	U.usrInits 
FROM
	dbo.dbFile F
INNER JOIN
	dbo.dbClient CL ON CL.clID = F.clID
LEFT OUTER JOIN 
	dbo.dbUser U ON U.usrID = F.filePrincipleID
LEFT OUTER JOIN
	dbContact C ON C.contID = F.fileSourceContact
LEFT OUTER JOIN
	dbUser U1 ON U1.usrID = F.fileSourceUser
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI ) X ON X.[cdCode] = F.[fileDepartment]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''SOURCE'' , @UI ) Y ON Y.[cdCode] = F.[fileSource]'

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- SOURCE CLAUSE
IF(@SOURCE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' WHERE F.fileSource = @SOURCE'
END

--- DEPARTMENT CLAUSE
IF(@DEPARTMENT IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' WHERE F.fileDepartment = @DEPARTMENT'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPARTMENT'
	END
END

--- FILE TYPE
IF(@TYPE IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' WHERE F.fileType = @TYPE'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND f.fileType = @TYPE'
	END
END

--- FILE STATUS
IF(@STATUS IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' WHERE F.fileStatus = @STATUS'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND f.fileSTATUS = @STATUS'
	END
END

--- START AND END DATE RANGE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' WHERE (F.Created >= @STARTDATE AND F.Created < @ENDDATE)'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND (F.Created >= @STARTDATE AND F.Created < @ENDDATE)'
	END
END


--- CONCATENATE THE SELECT & WHERE CLAUSES INTO THE SQL VARIABLE
SET @SQL = @SELECT + @WHERE

--- Debug Print
--- PRINT @SQL

exec sp_executesql @sql,
 N'
	@UI nvarchar(10),
	@SOURCE nvarchar(50),
	@DEPARTMENT nvarchar(50),
	@STARTDATE DateTime,
	@ENDDATE DateTime,
	@STATUS nvarchar(30),
	@TYPE nvarchar(30)',
	@UI,
	@SOURCE,
	@DEPARTMENT,
	@STARTDATE,
	@ENDDATE,
	@STATUS,
	@TYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilSource] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilSource] TO [OMSAdminRole]
    AS [dbo];

