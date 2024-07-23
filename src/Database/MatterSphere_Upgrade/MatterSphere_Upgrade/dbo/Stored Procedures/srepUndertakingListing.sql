CREATE PROCEDURE [dbo].[srepUndertakingListing]
(
	@UI uUICultureInfo = '{default}'
	, @UNDTYPE nvarchar(15) = NULL
)

AS 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(1000)

--- SET THE SELECT STATEMENT
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT     
	CL.clNo
	, F.fileNo
	, U1.usrInits
	, COALESCE(U.undRef, '''') as undRef
	, U.undDesc
	, U.undCompleted
	, U.undCompletedBy
	, U.undEstCompletion
	, CL.clName
	, dbo.GetCodeLookupDesc(''UNDERTYPE'', U.undType, @UI) AS cdDesc
	, U2.usrInits AS AuthByInits
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUndertakings U ON F.fileID = U.fileID 
INNER JOIN
	dbo.dbUser U1 ON F.filePrincipleID = U1.usrID
INNER JOIN
	dbo.dbUser U2 ON U.undAuthBy = U2.usrID
LEFT JOIN 
	dbo.GetCodeLookupDescription ( ''UNDERTYPE'' , @UI ) CL1 ON CL1.cdCode = U.undType '

---- SET THE WHERE STATEMENT
SET @WHERE = ' U.undActive = 1 '

--- UNDERTAKING TYPE CLAUSE
IF(@UNDTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND U.undType = @UNDTYPE '
END

--- BUILD THE WHERE CLAUSE
IF @WHERE <> ''
BEGIN
	SET @WHERE = N' WHERE ' + @WHERE
END

DECLARE @SQL nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @SQL = @SELECT + @WHERE

--- DEBUG PRINT
PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @UNDTYPE nvarchar(15)'
	, @UI
	, @UNDTYPE

