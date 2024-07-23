

CREATE PROCEDURE [dbo].[srepCliFileList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clNo, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.Created, 
	F.fileClosed, 
	F.fileType, 
        U.usrInits AS FeeInits, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS FileTypeDesc, 
	CL.clName, 
        U.usrFullName
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID  
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType
WHERE
	F.fileStatus LIKE ''%LIVE%'' AND
	CL.clNo = COALESCE(@CLNO, CL.clNo)'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50)', @UI, @CLNO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliFileList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliFileList] TO [OMSAdminRole]
    AS [dbo];

