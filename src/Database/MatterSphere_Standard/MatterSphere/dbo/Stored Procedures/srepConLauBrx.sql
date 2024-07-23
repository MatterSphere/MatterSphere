

CREATE PROCEDURE [dbo].[srepConLauBrx]
(
	@UI uUICultureInfo='{default}',
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS 

DECLARE @SQL nvarchar(3000)
DECLARE @SELECT nvarchar(1500)
DECLARE @WHERE nvarchar(1500)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	X.cdDesc as contTypeCode,
	C.contName, 
	C.contApproved, 
	C.contApprRevokedOn, 
	C.Created, 
	CA.contaddID, 
	A.addLine1, 
	A.addLine2, 
	A.addLine3
FROM         
	dbo.dbContact C
INNER JOIN
	dbo.dbContactAddresses CA ON C.contID = CA.contID
INNER JOIN
	dbo.dbAddress A ON CA.contaddID = A.addID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''CONTTYPE'' , @UI ) X ON X.cdCode = C.contTypeCode '

--- Build Where Clause
SET @WHERE = N'
	WHERE (C.ContTypeCOde = ''BARRISTER'' OR C.contTypeCode = ''EXPERT'') AND C.ContApproved = 1 '

--- @STARTDATE clause
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (C.Created >= @STARTDATE AND C.Created < @ENDDATE) '
END


--- BUILD THE SQL
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

--- Debug Print
-- print @SQL

exec sp_executesql @sql,
N'
	@UI nvarchar(10),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@STARTDATE,
	@ENDDATE	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConLauBrx] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConLauBrx] TO [OMSAdminRole]
    AS [dbo];

