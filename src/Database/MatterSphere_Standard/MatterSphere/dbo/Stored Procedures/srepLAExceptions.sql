

CREATE PROCEDURE [dbo].[srepLAExceptions]
(
	@UI uUICultureInfo = '{default}'
	, @CONTRACT nvarchar(15) = NULL
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nvarchar(1900)
DECLARE @WHERE nvarchar(2000)
DECLARE @ORDERBY nvarchar(100)


--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CL.clNo
	, CI.contDOB
	, CI.contSex
	, F.fileNo
	, A.addPostcode
    , CI.contEthnicOrigin
	, X.cdDesc AS EthnicDesc
	, U.usrFullName
	, FL.MatLAMatType
	, FL.MatLAPartI
    , FL.MatLAPartII
	, FL.MatEndPoint
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
LEFT OUTER JOIN
	dbo.dbAddress A ON CL.clDefaultAddress = A.addID 
LEFT OUTER JOIN
	dbo.dbContactIndividual CI ON CL.clDefaultContact = CI.contID 
LEFT OUTER JOIN
	dbo.dbFileLegal FL ON F.fileID = FL.fileID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''ETHNIC'', @UI) AS X ON X.cdCode = CI.contEthnicOrigin '

---- SET THE WHERE STATEMENT
SET @WHERE = ''

--- CONTRACT CLAUSE
IF(@CONTRACT IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' FL.MatLAContract = @CONTRACT '
END

--- FILE CLOSED START DATE AND END DATE
IF(@STARTDATE IS NOT NULL)
BEGIN
	IF(@WHERE <> '')
	BEGIN
		SET @WHERE = @WHERE + ' AND (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' (F.fileClosed >= @STARTDATE AND F.fileClosed < @ENDDATE) '
	END
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

-- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @CONTRACT nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime '
	, @UI
	, @CONTRACT 
	, @STARTDATE
	, @ENDDATE 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAExceptions] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAExceptions] TO [OMSAdminRole]
    AS [dbo];

