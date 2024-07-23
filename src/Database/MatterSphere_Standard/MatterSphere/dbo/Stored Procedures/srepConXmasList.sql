

CREATE PROCEDURE [dbo].[srepConXmasList] 
(
	@UI uUICultureInfo='{default}',
	@XMASCARD bit = null,
	@CREATEDFROM datetime = null
)

AS 

declare @sql nvarchar(3500)
declare @where nvarChar(2000)
declare @select nvarChar(2000)
declare @orderby nvarChar(1000)


--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	C.contaddressee,
	U.usrFullName AS FirmCont,
	C.contName,
	dbo.GetAddress(C.contDefaultAddress, '', '', @UI) AS Address,
	C.contxmascard,
	C.contisclient,
	C.Created
FROM
	dbContact C
LEFT OUTER JOIN
	dbContactIndividual CI ON C.contId = CI.contID
LEFT OUTER JOIN
	dbClient CL ON CL.clDefaultContact = C.contID
LEFT OUTER JOIN
	dbUser U ON
			CASE
				WHEN CL.feeUsrid IS NOT NULL THEN CL.feeUsrId
				ELSE C.CreatedBy
			END = U.UsrId '

--- Where Statement
set @where = ' WHERE C.contIsClient = 0 AND CI.contDOD IS NULL '

--- XMASCARD filter
if (@XMASCARD IS NOT NULL)
		set @where = @where + ' AND C.contXMASCard = @XMASCARD '

--- CREATEFROM filter
if (@CREATEDFROM IS NOT NULL)
	set @where = @where + ' AND C.Created >= @CREATEDFROM '

--- ORDER BY CLause
set @orderby = '
ORDER BY
	FirmCont,
	C.contName,
	Address'

--- join all the clauses together
set @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@orderby)

--- DEBUG PRINT
-- PRINT @SQL

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @XMASCARD bit
	, @CREATEDFROM datetime'
	, @UI
	, @XMASCARD
	, @CREATEDFROM

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConXmasList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConXmasList] TO [OMSAdminRole]
    AS [dbo];

