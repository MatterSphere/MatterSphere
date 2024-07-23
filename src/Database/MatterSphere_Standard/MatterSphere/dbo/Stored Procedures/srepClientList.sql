

CREATE PROCEDURE [dbo].[srepClientList]
(
	@UI uUICultureInfo = '{default}'
	, @FIRMCONT int = NULL
	, @SOURCE nvarchar(15) = NULL
	, @CLTYPE nvarchar(15) = NULL
	, @CLASSIFICATION nvarchar(15) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
	, @GROUPBY nvarchar(30) = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1500)

--- BUILD THE SELECT CLAUSE
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo 
	, CL.clName
	, X.[cdDesc] as clTypeDesc
	, CL.clSearch1
	, CL.clSearch2
	, CL.Created
	, U.usrFullName AS ContName
	, CASE
		WHEN A.addPostCode IS NOT NULL THEN A.addPostCode
		WHEN A1.AddPostCode IS NOT NULL THEN A1.AddPostCode
		ELSE NULL
	  END AS PostCode
	, CASE 
		WHEN CL.clSource = ''CONTACT'' THEN ''Contact: '' + C1.contName
		WHEN CL.clSource = ''USER'' THEN ''User: '' + SU.usrFullName
		ELSE Replace(Y.[cdDesc],''%CLIENT%'',''Client'')
	  END AS Source
	, CASE
		WHEN @GROUPBY = ''FC'' THEN ''Grouped by Firm Contact : '' + U.usrFullName
		WHEN @GROUPBY = ''SC'' THEN ''Grouped by Source : '' + Y.[cdDesc]
		WHEN @GROUPBY = ''CT'' THEN ''Grouped by Client Type : '' + X.[cdDesc]
	END AS GroupBy
FROM         
	dbo.dbClient CL 
LEFT OUTER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID
LEFT OUTER JOIN
	dbo.dbAddress A ON A.addID = CL.clDefaultAddress
LEFT OUTER JOIN
	dbo.dbContact C ON C.contID = CL.clDefaultContact
LEFT OUTER JOIN
	dbo.dbAddress A1 ON A1.addID = C.contDefaultAddress
LEFT OUTER JOIN
	dbo.dbContact C1 ON C1.contID = CL.clSourceContact
LEFT OUTER JOIN
	dbo.dbUser SU ON SU.usrID = CL.clSourceUser
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''CLType'' , @UI ) X ON X.[cdCode] = CL.[clTypeCode] 
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''SOURCE'' , @UI ) Y ON Y.[cdCode] = CL.clSource '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- PRE CLIENT CLASSIFICATION CLAUSE
IF(@CLASSIFICATION IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' CL.clPreClient = @CLASSIFICATION '
END

--- FIRM CONTACT CLAUSE
IF(@FIRMCONT IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.feeUsrID = @FIRMCONT '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' CL.feeUsrID = @FIRMCONT '
	END
END

--- CLIENT SOURCE CLAUSE
IF(@SOURCE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.clSource = @SOURCE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' CL.clSource = @SOURCE '
	END
END

--- CLIENT TYPE CLAUSE
IF(@CLTYPE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND CL.clTypeCode = @CLTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' CL.clTypeCode = @CLTYPE '
	END
END

--- CLIENT CREATED CLAUSE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF (@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE) ' 
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (CL.Created >= @STARTDATE AND CL.Created < @ENDDATE) '
	END
END

--- BUILD THE WHERE CLAUSE
IF (@WHERE <> '')
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

--- DEBUG PRINT
-- PRINT @SQL

--- EXECUTE THE PROCEDURE
EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @FIRMCONT int
	, @SOURCE nvarchar(15)
	, @CLTYPE nvarchar(15)
	, @CLASSIFICATION nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @GROUPBY nvarchar(30)'
	, @UI
	, @FIRMCONT
	, @SOURCE
	, @CLTYPE
	, @CLASSIFICATION
	, @STARTDATE
	, @ENDDATE
	, @GROUPBY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientList] TO [OMSAdminRole]
    AS [dbo];

