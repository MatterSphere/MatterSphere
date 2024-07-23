

CREATE PROCEDURE [dbo].[srepFilAssocList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null,
	@FILENO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(MAX)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clNo, 
	CL.clName, 
	F.fileNo, 
	F.fileDesc, 
	C.contName, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.assocType, '''') + ''~'') AS AssocType, 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(C.contTypeCode, '''') + ''~'') AS ContType, 
	MAX(CN.contNumber) AS contNumber, 
	MAX(D.Created) AS LastDocDate, 
	AD.addLine1
FROM    
	dbo.dbAssociates A
INNER JOIN
        dbo.dbContact C ON A.contID = C.contID 
INNER JOIN
        dbo.dbClient CL
INNER JOIN
        dbo.dbFile F ON CL.clID = F.clID ON A.fileID = F.fileID 
INNER JOIN
        dbo.dbAddress AD ON dbo.GetAssocAddressID(A.assocID) = AD.addID 
LEFT OUTER JOIN
        dbo.dbContactNumbers CN ON C.contID = CN.contID 
LEFT OUTER JOIN
        dbo.dbDocument D ON A.assocID = D.assocID
LEFT JOIN dbo.GetCodeLookupDescription ( ''SUBASSOC'', @UI ) CL1 ON CL1.[cdCode] = A.assocType
LEFT JOIN dbo.GetCodeLookupDescription ( ''CONTTYPE'', @UI ) CL2 ON CL2.[cdCode] = C.contTypeCode
WHERE
	A.assocActive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo) AND
	F.fileNo = COALESCE(@FILENO, F.fileNo)
GROUP BY 
	CL.clNo, 
	CL.clName, 
	F.fileNo, 
	F.fileDesc, 
	C.contName, 
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(A.assocType, '''') + ''~''), 
	COALESCE(CL2.cdDesc, ''~'' + NULLIF(C.contTypeCode, '''') + ''~''), 
	AD.addLine1'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50), @FILENO nvarchar(50)', @UI, @CLNO, @FILENO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAssocList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilAssocList] TO [OMSAdminRole]
    AS [dbo];

