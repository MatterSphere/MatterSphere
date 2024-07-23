

CREATE PROCEDURE [dbo].[srepFileNoSecurity] 
(
	@UI uUICultureInfo = '{default}'
	, @STARTDATE datetime = null
	, @ENDDATE datetime = null
	, @TYPE nvarChar(50) = null
	, @FEEEARNER int = null
)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)


--- Select Statement for the base Query
SET @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	C.clNo + ''/'' + F.fileNo as Ref
	, [dbo].[GetContactAssociatedAsClient] (C.clid) as [Client Contact]
	, F.fileDesc
	, U.usrFullName
	, FE.evExtended as evdesc
	, FE.evWhen
	, FEE.UsrFullName as FeeEarner
FROM
	dbo.dbFile F
INNER JOIN
	dbo.dbFileEvents FE ON F.fileID = FE.fileID 
INNER JOIN
	dbo.dbUser FEE On F.FilePrincipleID = FEE.UsrID
INNER JOIN
	dbo.dbUser U ON FE.evusrID = U.usrID
INNER JOIN
	dbo.dbClient C ON F.clID = C.clID'

---- Debug Print
---PRINT @SELECT

---- Start of WHERE clause

--- Mandatory Filter
SET @Where = N'
 WHERE
	FE.evtype = ''SECAUDIT''  AND 
	(SUBSTRING(FE.evExtended, 1, 21)  = ''Audit to be Confirmed'' OR
	 SUBSTRING(FE.evExtended, 14, 19) = ''Audit was Cancelled'')'
	 --SUBSTRING(FE.evExtended = ''Audit Cancelled'')'

---- Date range filter
IF (@STARTDATE IS NOT NULL)
BEGIN
	SET @Where = @Where + N' AND (FE.evWhen >= @STARTDATE AND FE.evWhen < @ENDDATE)'
END

--- Type filter
IF (@TYPE IS NOT NULL)
BEGIN
	SET @Where = @Where + N' AND F.fileType = @TYPE' 
END

-- Fee Earner
IF (@FEEEARNER IS NOT NULL)
BEGIN
	SET @Where = @Where + N' AND F.filePrincipleID = @FEEEARNER'
END

---- Build OrderBy Clause
set @orderby = N' ORDER BY FeeEarner, Ref, FE.evWhen'

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby 

--- Debug Print
--  print @sql


exec sp_executesql @sql, 
N'
	@UI nvarchar(15)
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @TYPE nvarChar(50)
	, @FEEEARNER int'
	, @UI
	, @STARTDATE
	, @ENDDATE
	, @TYPE
	, @FEEEARNER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileNoSecurity] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileNoSecurity] TO [OMSAdminRole]
    AS [dbo];

