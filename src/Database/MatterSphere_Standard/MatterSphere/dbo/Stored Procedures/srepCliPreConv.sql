

CREATE PROCEDURE [dbo].[srepCliPreConv]
(
	@UI uUICultureInfo='{default}',
	@FIRMCONT int = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref, 
	CL.clPreClientConvDate, 
	CL.Created, 
	DATEDIFF(d, CL.created, CL.clPreClientConvDate) AS daysdiff, 
	CL.clName, 
	U.usrInits AS FirmContInits, 
	U.usrFullName AS FirmContName, 
	CL.clPreClient
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
LEFT OUTER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID
WHERE
	CL.clPreClient = 1 AND
	COALESCE(CL.feeusrid, '''') = COALESCE(@FIRMCONT, CL.feeusrid, '''') AND
	(CL.clPreClientConvDate BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), CL.clPreClientConvDate) AND COALESCE(dbo.GetEndDate(@ENDDATE), CL.clPreClientConvDate))'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @FIRMCONT int, @STARTDATE datetime, @ENDDATE datetime', @UI, @FIRMCONT, @STARTDATE, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliPreConv] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliPreConv] TO [OMSAdminRole]
    AS [dbo];

