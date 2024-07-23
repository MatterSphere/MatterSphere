CREATE PROCEDURE [dbo].[srepDocListFiles]
(
	@UI uUICultureInfo = '{default}'
	, @FILEID bigint 
)

AS 

DECLARE @SQL nvarchar(2100)

--- SET THE SELECT STATEMENT
SET @SQL = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	CASE
		WHEN D.docAuthored IS NULL THEN D.Created
		ELSE D.DocAuthored
	ENd AS Created, 
	D.Createdby, 
	replace(F.fileDesc, char(13) + char(10), '''') as fileDesc, 
	CL.clNo, 
	F.fileNo, 
	U.usrInits, 
	D.docDesc,
	D.docID,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(D.docType, '''') + ''~'') AS DocTypeDesc,
	D.docDirection, 
	CL.clName, 
	U.usrFullName, 
	D.docID
FROM    
	dbo.dbClient CL 
INNER JOIN
    dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
    dbo.dbDocument D ON F.fileID = D.fileID 
INNER JOIN
    dbo.dbUser U ON D.Createdby = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''DOCTYPE'', @UI ) CL1 ON CL1.[cdCode] =  D.docType
WHERE
	D.fileId = @FILEID AND
	D.docDeleted = 0 '

--- DEBUG PRINT
--PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI uUICultureInfo
	, @FILEID bigint'
	, @UI
	, @FILEID

