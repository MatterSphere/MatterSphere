CREATE PROCEDURE [dbo].[srepDocsUsingPrecs]
(
	@UI uUICultureInfo='{default}',
	@USER bigint = null,
	@DEPARTMENT nvarchar(50) = null,
	@FILETYPE nvarchar(50) = null,
	@FUNDTYPE nvarchar(50) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null,
	@SUMMARY bit = 0
) 

AS 

DECLARE @SQL nvarchar(MAX)

--- Select Statement for the base Query
SET @SQL = N'
SELECT
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(F.fileType, '''') + ''~'') AS fileTypeDesc, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(F.fileFundCode, '''') + ''~'') AS fundTypeDesc,
	COALESCE(CL3.cdDesc, ''~'' + NULLIF(F.fileDepartment, '''') + ''~'') AS fileDepartmentDesc, 
        U.usrFullName, 
	U.usrInits, 
	DOC.docID, 
	COALESCE(DOC.docAuthored, DOC.Created) AS Created,
	P.precTitle,
	P.precDesc,
	P1.precTitle AS baseTitle,
	P1.precDesc AS baseDesc,
	CASE 
	WHEN @SUMMARY = 0 THEN 0 ELSE 1 END AS Summary,
	CASE
	WHEN DOC.docPrecID IS NOT NULL THEN 1 ELSE 0 END AS TotalPrecs
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbDocument DOC ON F.fileID = DOC.fileID 
LEFT OUTER JOIN
	dbo.dbUser U ON DOC.CreatedBy = U.usrID 
LEFT OUTER JOIN
	dbo.dbPrecedents P ON DOC.docprecID = P.precID
LEFT OUTER JOIN
	dbo.dbPrecedents P1 ON DOC.docBasePrecID = P1.precID
INNER JOIN
	dbClient CL ON CL.clID = F.clID
LEFT JOIN dbo.GetCodeLookupDescription ( ''FILETYPE'', @UI ) CL1 ON CL1.[cdCode] = F.fileType
LEFT JOIN dbo.GetCodeLookupDescription ( ''FUNDTYPE'', @UI ) CL2 ON CL2.[cdCode] = F.fileFundCode
LEFT JOIN dbo.GetCodeLookupDescription ( ''DEPT'', @UI ) CL3 ON CL3.[cdCode] = F.fileDepartment
WHERE
	DOC.docPrecID IS NOT NULL AND
	COALESCE(DOC.CreatedBy, -1) = COALESCE(@USER, U.usrID, -1) AND
	COALESCE(F.fileDepartment, '''') = COALESCE(@DEPARTMENT, F.fileDepartment, '''') AND
	COALESCE(F.fileType, '''') = COALESCE(@FILETYPE, F.fileType, '''') AND
	COALESCE(F.fileFundCode, '''') = COALESCE(@FUNDTYPE, F.fileFundCode, '''') AND
	(DOC.Created >= COALESCE(@STARTDATE, DOC.Created) AND DOC.Created <= COALESCE(@ENDDATE, DOC.Created))
ORDER BY
	DOC.Created'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @USER nvarchar(50), @DEPARTMENT nvarchar(50), @FILETYPE nvarchar(50), @FUNDTYPE nvarchar(50), @STARTDATE datetime, @ENDDATE datetime, @SUMMARY bit', @UI, @USER, @DEPARTMENT, @FILETYPE, @FUNDTYPE, @STARTDATE, @ENDDATE, @SUMMARY

