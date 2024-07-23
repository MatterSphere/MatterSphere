

CREATE PROCEDURE [dbo].[srepCliXmasCard]
(
	@UI uUICultureInfo='{default}',
	@CLTYPE nvarchar(50) = null,
	@CLASSIFICATION nvarchar(50) = null,
	@XMASTRUE nvarchar(10) = null,
	@XMASFALSE nvarchar(10) = null,
	@XMASALL nvarchar(10) = null
)

AS 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(1000)

--- Select Statement for the base Query
SET @SELECT = N'
SELECT     
	U.usrID, 
	U.usrInits, 
	U.usrFullName, 
	CL.clNo, 
	CL.clName,
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(CL.cltypeCode, '''') + ''~'') AS ClTypeDesc, 
	CL.clPreClient, 
	B.brName, 
	C.contXMASCard
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbBranch B ON CL.brID = B.brID 
INNER JOIN
	dbo.dbContact C ON CL.clDefaultContact = C.contID 
INNER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''CLTYPE'', @UI ) CL1 ON CL1.[cdCode] = CL.cltypeCode'

--- Build the Where statement
SET @WHERE = N' 
WHERE
	CL.clTypeCode = COALESCE(@CLTYPE, CL.clTypeCode) AND
	CL.clPreClient = COALESCE(@CLASSIFICATION, CL.clPreClient) '

IF @XMASTRUE = 'True'
	BEGIN
		SET @WHERE = @WHERE + 'AND C.contXMASCard = ''1'' '
	END
ELSE
IF @XMASFALSE = 'True'
	BEGIN
		SET @WHERE = @WHERE + 'AND C.contXMASCard = ''0'' '
	END
ELSE
IF @XMASALL = 'True'
	BEGIN
		SET @WHERE = @WHERE + 'AND (C.contXMASCard = ''1'' OR C.contXMASCard = ''0'') '
	END

DECLARE @SQL nvarchar(4000)

--- Join all statements together
SET @SQL = @SELECT + @WHERE

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLTYPE nvarchar(50), @CLASSIFICATION nvarchar(50), @XMASTRUE nvarchar(10), @XMASFALSE nvarchar(10), @XMASALL nvarchar(10)', @UI, @CLTYPE, @CLASSIFICATION, @XMASTRUE, @XMASFALSE, @XMASALL

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliXmasCard] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliXmasCard] TO [OMSAdminRole]
    AS [dbo];

