

CREATE PROCEDURE [dbo].[srepDicJobList]
(
	@TYPIST int = Null
	, @DICSTATUS nvarchar(15) = NULL
	, @AUTHOR int = NULL
	, @PRIORITY nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1400)
DECLARE @ORDERBY nvarchar(100)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	D.dicID, 
    dbo.FormatSeconds(D.dicLength) as dicLength, 
	D.dicStatus, 
	C.CLName,
	D.dicAssocName,
	F.FileDesc, 
	D.dicTitle, 
	D.dicPriority, 
	AU.usrFullName AS Author, 
	TY.usrFullName AS Typist, 
	DP.dicPoolName,
	D.dicReference as Ref,
	D.dicCreated,
	P.PrecTitle + ''['' + P.PrecDesc + '']'' AS PrecDesc
FROM         
	dbo.dbDictations  D
LEFT OUTER JOIN
	dbo.dbDictationsPools DP ON D.dicPoolID = DP.dicPoolID 
LEFT OUTER JOIN
	dbo.dbUser TY ON D.dicTypistUserID = TY.usrID 
LEFT OUTER JOIN
	dbo.dbUser AU ON D.dicAuthorUserID = AU.usrID 
LEFT OUTER JOIN
	dbo.dbClient C ON D.clid = C.clid
LEFT OUTER JOIN
	dbo.dbFile F ON D.fileid = F.fileID
LEFT OUTER JOIN
	dbo.dbPrecedents P ON P.precID = D.dicPrecID '

--- SET THE WHERE CLAUSE
SET @WHERE = ''

--- TYPIST CLAUSE
IF(@TYPIST IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' D.dicTypistUserID = @TYPIST '
END

--- AUTHOR CLAUSE
IF(@AUTHOR IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND D.dicAuthorUserID = @AUTHOR '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' D.dicAuthorUserID = @AUTHOR '
	END
END

--- PRIORITY CLAUSE
IF(@PRIORITY IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND D.dicPriority = @PRIORITY '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' D.dicPriority = @PRIORITY '
	END
END

--- DICSTATUS CLAUSE
IF(@DICSTATUS IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND D.dicStatus = @DICSTATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' D.dicStatus = @DICSTATUS '
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

--- BUILD THE ORDER BY CLAUSE
SET @ORDERBY = ' ORDER BY D.dicPriority DESC '

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@TYPIST int
	, @DICSTATUS nvarchar(15)
	, @AUTHOR int
	, @PRIORITY nvarchar(15)'
	, @TYPIST
	, @DICSTATUS
	, @AUTHOR
	, @PRIORITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDicJobList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepDicJobList] TO [OMSAdminRole]
    AS [dbo];

