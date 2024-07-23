

CREATE PROCEDURE [dbo].[srepConXmasLabs]
(
	@UI uUICultureInfo='{default}'
)

AS 

declare @SQL nvarchar(2500)

--- Select Statement for the base Query
set @SQL = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	C.contaddressee,
	U.usrInits AS FirmCont,
	C.contName,
	dbo.GetAddress(C.contDefaultAddress, char(10), @UI) AS Address
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
			END = U.UsrId
WHERE
	C.contXMASCard = 1 AND C.ContIsClient = 0 AND CI.contDOD IS NULL
ORDER BY
	FirmCont,
	C.contName,
	Address'

--- Debug Print
-- print @sql


exec sp_executesql @sql, 
N'
	@UI nvarchar(10)',
	@UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConXmasLabs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConXmasLabs] TO [OMSAdminRole]
    AS [dbo];

