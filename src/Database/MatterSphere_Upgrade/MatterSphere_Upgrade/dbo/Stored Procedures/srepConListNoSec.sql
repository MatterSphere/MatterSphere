CREATE PROCEDURE [dbo].[srepConListNoSec]
(
	@UI uUICultureInfo='{default}',
	@DEPARTMENT nvarchar(50) = null,
	@FILETYPE nvarchar(50) = null,
	@FEEEARNER nvarchar(50) = null
)

AS 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(900)
DECLARE @ORDERBY nvarchar(100)

--- Set the Select Statement
SET @SELECT = N'
SELECT
	CL.clNo,
	F.fileNo,
	CL.clName,
	F.fileDesc,
	C.contName,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(C.contTypeCode, '''') + ''~'') AS contType,
	C.Created,
	U.usrFullName,
	U1.usrFullName
FROM
	dbClient CL
INNER JOIN
	dbFile F ON F.clID = CL.clID
INNER JOIN
	dbAssociates A ON A.fileID = F.fileID
INNER JOIN
	dbContact C ON C.contID = A.contID
LEFT OUTER JOIN
	dbContactSecurity CS on CS.contID = C.contID
INNER JOIN
	dbUser U ON U.usrID = C.CreatedBy
INNER JOIN
	dbUser U1 ON U1.usrID = F.filePrincipleID
LEFT JOIN dbo.GetCodeLookupDescription ( ''CONTTYPE'', @UI ) CL1 ON CL1.[cdCode] = C.contTypeCode'

--- Set the Where Statement
SET @WHERE = ' 
WHERE
	CS.contID IS NULL OR
	CS.ImageID1 IS NULL AND
	CS.ImageID2 IS NULL AND
	CS.ImageID3 IS NULL AND
	CS.ImageID4 IS NULL AND
	CS.Question IS NULL AND
	CS.PassPhrase IS NULL AND
	F.fileStatus LIKE ''LIVE%'' AND
	F.fileDepartment = COALESCE(@DEPARTMENT, F.fileDepartment) AND
	F.fileType = COALESCE(@FILETYPE, F.fileType) AND
	COALESCE(F.filePrincipleID, '''') = COALESCE(@FEEEARNER, F.filePrincipleID, '''') '

--- Set the Order By Statement
SET @ORDERBY = ' 
ORDER BY
	CL.clNo'

DECLARE @SQL nvarchar(4000)
--- Join the Statements together
SET @SQL = @SELECT + @WHERE + @ORDERBY

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @DEPARTMENT nvarchar(50), @FILETYPE nvarchar(50), @FEEEARNER nvarchar(50)', @UI, @DEPARTMENT, @FILETYPE, @FEEEARNER
