

CREATE PROCEDURE [dbo].[srepArchiveListing]
(
	@UI uUICultureInfo='{default}',
	@ARCHIVETYPE nvarchar(50) = null,
	@LOCATION nvarchar(50) = null,
	@AUTHBY bigint = null
)

AS 

declare @SQL nvarchar(4000)

--- Select Statement for the base Query
set @SQL = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	A.archRef, 
	A.archDesc, 
	A.archType, 
	A.archInStorage, 
        A.archStorageLoc, 
	A.Created, 
	A.archDeleted, 
	U.usrInits, 
	U.usrFullName, 
        A.archAuthBy, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.archType, '''') + ''~'') AS ArchTypeDesc, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(A.archstorageloc, '''') + ''~'') AS ArchLocDesc,
	CASE
	WHEN A.archNote > '''' THEN ''Notes: '' + A.archNote
	END AS archNote
FROM         
	dbo.dbClient CL
INNER JOIN
        dbo.dbArchive A ON CL.clID = A.clID 
INNER JOIN
        dbo.dbUser U ON CL.feeusrID = U.usrID
LEFT JOIN dbo.GetCodeLookupDescription ( ''ARCHTYPE'', @UI ) CL1 ON CL1.[cdCode] =  A.archType
LEFT JOIN dbo.GetCodeLookupDescription ( ''LOCTYPE'', @UI ) CL2 ON CL2.[cdCode] =  A.archstorageloc
WHERE
	A.archActive = 1 AND
	A.archType = COALESCE(@ARCHIVETYPE, A.archType) AND
	A.archStorageLoc = COALESCE(@LOCATION, A.archStorageLoc) AND
	A.archAuthBy = COALESCE(@AUTHBY, A.archAuthBy)
ORDER BY
	A.Created ASC'

--- Debug Print
print @sql


exec sp_executesql @sql, N'@ARCHIVETYPE uCodeLookup, @LOCATION uCodeLookup, @AUTHBY bigint, @UI nvarchar(10)', @ARCHIVETYPE, @LOCATION, @AUTHBY, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepArchiveListing] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepArchiveListing] TO [OMSAdminRole]
    AS [dbo];

