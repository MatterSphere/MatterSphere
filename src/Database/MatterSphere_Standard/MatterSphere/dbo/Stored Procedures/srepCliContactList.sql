

CREATE PROCEDURE [dbo].[srepCliContactList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clno, 
	CL.clname, 
	C.contName, 
	A.addLine1, 
	A.addLine2, 
	A.addLine3, 
	A.addLine4, 
        A.addLine5, 
	A.addPostcode, 
	C.contXMASCard, 
	C.contApproved, 
	C.contGrade, 
        CLU.cdType, 
	C.contAddFilter, 
	C.contTypeCode, 
	CLU.cdDesc, 
	CLU.cdCode, 
        CL.clNo
FROM    
	dbo.dbContact C
INNER JOIN
        dbo.dbCodeLookup CLU ON C.contTypeCode = CLU.cdCode
INNER JOIN
        dbo.dbAddress A ON C.contDefaultAddress = A.addID 
INNER JOIN
        dbo.dbClientContacts CC ON C.contID = CC.contID
INNER JOIN
        dbo.dbClient CL ON CC.clID = CL.clID
WHERE
	CL.clNo = COALESCE(@CLNO, CL.clNo)
ORDER BY
	C.contName'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50)', @UI, @CLNO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliContactList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliContactList] TO [OMSAdminRole]
    AS [dbo];

