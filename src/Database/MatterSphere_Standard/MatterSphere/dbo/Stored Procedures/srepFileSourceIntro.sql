

CREATE PROCEDURE [dbo].[srepFileSourceIntro]
(
	@UI uUICultureInfo='{default}',
	@FeeEarner int = null,
	@begindate datetime=null,
	@enddate datetime=null
)

AS 

declare @Select nvarchar(1700)
declare @Where nvarchar(1700)
declare @groupby nvarchar(50)
declare @orderby nvarchar(50)

--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT     
	CLUP.cdDesc, 
	Count(CLUP.cdDesc) as srcecount
FROM 
	dbo.dbUser U
LEFT OUTER JOIN
	dbFile F ON F.filePrincipleID = U.usrID
LEFT OUTER JOIN
	dbCodeLookup CLUP ON CLUP.cdCode = F.fileSource '

---- Debug Print
-- PRINT @SELECT

---- Build Where Clause
SET @WHERE = N' WHERE CLUP.cdType = ''SOURCE'''

if (@FeeEarner IS NOT NULL)
BEGIN
	SET @where = @where + ' AND U.usrid = @feeearner '
END

-- date filters
if (@BEGINDATE IS NOT NULL)
BEGIN
	SET @where = @where + ' AND (F.Created >= @BEGINDATE AND F.Created < @ENDDATE) '	
END  

---- Build Group By Clause
set @groupby = N' GROUP BY CLUP.cdDesc '

---- Build OrderBy Clause
set @orderby = N' ORDER BY CLUP.cdDesc '

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@groupby) + Rtrim(@orderby)

-- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	, @FeeEarner int
	, @begindate datetime
	, @enddate datetime '
	, @UI
	, @FeeEarner
	, @begindate
	, @enddate

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileSourceIntro] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileSourceIntro] TO [OMSAdminRole]
    AS [dbo];

