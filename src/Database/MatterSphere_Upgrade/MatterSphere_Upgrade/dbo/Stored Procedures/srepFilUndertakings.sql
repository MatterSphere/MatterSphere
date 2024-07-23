CREATE PROCEDURE [dbo].[srepFilUndertakings]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null,
	@FILENO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(max)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
	U.usrFullName, 
	UND.undRef, 
	UND.undDesc, 
        UND.undAuthBy, 
	UND.undCompleted, 
	UND.undCompletedBy, 
	UND.undEstCompletion, 
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(UND.undType, '''') + ''~'') AS undType,
	CL.clName,
	U1.usrInits AS AuthByInits, 
        U1.usrFullName AS AuthByFullName
FROM         
	dbo.dbClient CL
INNER JOIN
        dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUndertakings UND ON F.fileID = UND.fileID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID  
INNER JOIN
	dbo.dbUser U1 ON UND.undAuthBy = U1.usrID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''UNDERTYPE'' , @UI ) CL1 ON CL1.cdCode = UND.undType
WHERE
	UND.undactive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo) AND
	F.fileNo = COALESCE(@FILENO, F.fileNo)'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50), @FILENO nvarchar(50)', @UI, @CLNO, @FILENO
