

CREATE PROCEDURE [dbo].[srepUserList]
(
	@USER bigint = null
)

AS 

DECLARE @SQL nvarchar(4000)
DECLARE @SELECT nvarchar(2000)
DECLARE @WHERE nvarchar(500)
DECLARE @ORDERBY nvarchar(100)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT 
	U.usrInits,
	U.usrFullName,
	U.usrEmail,
	U.usrExtension,
	U.usrDDI,
	U1.usrFullName AS UserWorksFor,
	B.brName,
	P.printName
FROM 
	dbUser U
LEFT OUTER JOIN
	dbUser U1 ON U.usrWorksFor = U1.usrID
LEFT OUTER JOIN
	dbBranch B ON B.brID = U.brID
LEFT OUTER JOIN
	dbPrinter P ON P.printID = U.usrprintID '

-- build the where clause
SET @WHERE = N'
WHERE
	U.usrID > 0 AND
	U.usrActive = 1 '

-- User filter
IF (@USER IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND U.usrID = @USER '
END

-- order by clause
SET @ORDERBY = N' ORDER BY U.usrInits '

-- now build the whole thing
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- Debug Print
-- PRINT @SQL

exec sp_executesql @sql, N'
	@USER bigint',
	@USER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepUserList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepUserList] TO [OMSAdminRole]
    AS [dbo];

