

CREATE PROCEDURE [dbo].[srepCliSource]
(
	@UI uUICultureInfo = '{default}'
	, @SOURCE nvarchar(15) = NULL
	, @STARTDATE DateTime = NULL
	, @ENDDATE DateTime = NULL
	, @DEPT	nvarchar(15) = NULL
	, @TYPE nvarchar(15) = NULL
	, @STATUS nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1500)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT DISTINCT
	CL.clNo
	, CL.clName
	, CASE
		WHEN CL.clSource IS NULL THEN ''(Not Set)''
		ELSE Replace(X.[cdDesc],''%CLIENT%'',''Client'')
	  END AS Source
	, C.contName AS SourceCont
	, U.usrFullName AS SourceUser
	, CASE
		WHEN CL.clSource = ''CONTACT'' THEN 1
		WHEN CL.clSource = ''USER'' THEN 2
		ELSE 3
	END AS User_Contact
	, U1.usrInits AS FirmCont
	, F.Created AS Opened
FROM 
	dbClient CL
LEFT OUTER JOIN
	dbContact C ON C.contID = CL.clSourceContact
LEFT OUTER JOIN
	dbUser U ON U.usrID = CL.clSourceUser
LEFT OUTER JOIN
	dbUser U1 ON U1.usrID = CL.feeusrid
INNER JOIN
	dbFile F ON CL.clID = F.clID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''SOURCE'', @UI ) X ON X.[cdCode] = CL.[clSource] '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- CLIENT SOURCE OF BUSINESS CLAUSE
IF(@SOURCE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' CL.clSource = @SOURCE'
END

--- DEPARTMENT Filter
IF(@DEPT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileDepartment = @DEPT'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileDepartment = @DEPT'
	END
END

--- FILE TYPE Filter
IF(@TYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileType = @TYPE'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' F.fileType = @TYPE'
	END
END

--- @STATUS filter
if (@STATUS is not null)
begin
	if (@where <> '')
	begin
		set @where = @where + ' AND f.fileStatus = @STATUS'
	end
	else
	begin
		set @where = @where + ' f.fileStatus = @STATUS'
	end
end

--- CLIENT CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE)'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE)'
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @SOURCE nvarchar(15)
	, @STARTDATE DateTime
	, @ENDDATE DateTime
	, @DEPT nvarchar(15)
	, @TYPE nvarchar(15)
	, @STATUS nvarchar(15)'
	, @UI
	, @SOURCE
	, @STARTDATE
	, @ENDDATE
	, @DEPT
	, @TYPE
	, @STATUS

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliSource] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliSource] TO [OMSAdminRole]
    AS [dbo];

