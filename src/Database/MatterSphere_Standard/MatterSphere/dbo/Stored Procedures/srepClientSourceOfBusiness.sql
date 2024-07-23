

CREATE PROCEDURE [dbo].[srepClientSourceOfBusiness]
(
	@UI uUICultureInfo = '{default}'
	, @SOURCE nvarchar(15) = NULL
	, @STARTDATE DateTime = NULL
	, @ENDDATE DateTime = NULL
	, @CLIENTTYPE int = NULL
	, @PRECLIENT int = NULL
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
	, CT.cdDesc as ClienType
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
	, CL.Created AS ClientCreated
	, CASE
		WHEN CL.clPreClient = 1 THEN ''Yes''
		ELSE ''''
	  END AS clPreClient
FROM 
	dbClient CL
LEFT OUTER JOIN
	dbContact C ON C.contID = CL.clSourceContact
LEFT OUTER JOIN
	dbUser U ON U.usrID = CL.clSourceUser
LEFT OUTER JOIN
	dbUser U1 ON U1.usrID = CL.feeusrid
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''CLTYPE'', @UI ) CT ON CT.[cdCode] = CL.[ClTypeCode]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''SOURCE'', @UI ) X ON X.[cdCode] = CL.[clSource]'


--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- CLIENT SOURCE OF BUSINESS CLAUSE
IF(@SOURCE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' WHERE CL.clSource = @SOURCE'
END


--- FILE TYPE Filter
IF(@CLIENTTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.clTypeCode = @CLIENTTYPE'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' WHERE CL.clTypeCode = @CLIENTTYPE'
	END
END

-- Pre Client
IF (@PRECLIENT IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.clPreClient = @PRECLIENT'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' WHERE CL.clPreClient = @PRECLIENT'
	END
END


--- CLIENT CREATED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE)'
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' WHERE (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE)'
	END
END


DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE + ' OPTION (Maxdop 1)'

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @SOURCE nvarchar(15)
	, @STARTDATE DateTime
	, @ENDDATE DateTime
	, @CLIENTTYPE int
	, @PRECLIENT int'
	, @UI
	, @SOURCE
	, @STARTDATE
	, @ENDDATE
	, @CLIENTTYPE
	, @PRECLIENT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientSourceOfBusiness] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientSourceOfBusiness] TO [OMSAdminRole]
    AS [dbo];

