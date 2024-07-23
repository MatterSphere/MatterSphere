

CREATE PROCEDURE [dbo].[srepClArchList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null
)

AS 

declare @SQL nvarchar(4000)

--- Select Statement for the base Query
set @SQL = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	A.archRef, 
	A.archDestroyDate, 
	A.archDesc,
	A.archInStorage, 
	A.Created,
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS ArchTypeDesc, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archstorageloc, '''') + ''~'') AS ArchLocDesc,
	CASE
	WHEN A.archNote > '''' THEN ''Notes: '' + A.archNote
	END AS archNote,
	A.archActive,
	U.usrInits AS AuthInits
FROM         
	dbo.dbClient CL
INNER JOIN
        dbo.dbArchive A ON CL.clID = A.clID
LEFT OUTER JOIN
	dbo.dbUser U ON A.archAuthBy = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''ARCHTYPE'', @UI ) CL1 ON CL1.[cdCode] =  A.archType
LEFT JOIN dbo.GetCodeLookupDescription ( ''LOCTYPE'', @UI ) CL2 ON CL2.[cdCode] =  A.archstorageloc
WHERE
	A.archActive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo)
ORDER BY
	A.Created'

--- Debug Print
print @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50)', @UI, @CLNO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClArchList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClArchList] TO [OMSAdminRole]
    AS [dbo];

