

CREATE PROCEDURE [dbo].[srepCliSrceTop40]
(
	@UI uUICultureInfo='{default}',
	@FEEEARNER nvarchar(50) = null,
	@CLASSIFICATION nvarchar(20) = null,
	@STARTDATE datetime = Null,
	@ENDDATE datetime = Null
)

AS 

declare @sql nvarchar(4000)
declare @Select nvarchar(2500)
declare @Where nvarchar(2000)
declare @GROUPBY nvarchar(500)
declare @OrderBy nvarchar(500)


--- Select Statement for the base Query
set @Select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT TOP 40
	CLUP.cdDesc,
	Count(CLUP.cdDesc) AS SrcCount
FROM      
	dbo.dbClient CL
INNER JOIN
	dbo.dbCodeLookup CLUP ON CL.clSource = CLUP.cdCode '

--- Build Where Clause
SET @WHERE = ' WHERE CLUP.cdType = ''SOURCE'' '

if (@FEEEARNER IS NOT NULL)
BEGIN
     set @where = @where + ' AND CL.feeUsrId = @FEEEARNER '
END

if (@CLASSIFICATION IS NOT NULL)
BEGIN
     set @where = @where + ' AND CL.clPreClient = @CLASSIFICATION '
END

if (@STARTDATE IS NOT NULL)
BEGIN
     set @where = @where + ' AND (CL.created >= @STARTDATE AND CL.Created < @ENDDATE) '
END

--- Build the WHERE Clause
Set @GROUPBY = N'
GROUP BY
	CLUP.cdDesc '

Set @OrderBy = ' ORDER BY 2 Desc'

--- Add Statements together
set @sql = Rtrim(@Select) + Rtrim(@where) + Rtrim(@GROUPBY) + @OrderBy

--- Debug Print
-- print @sql


exec sp_executesql @sql, 
N'
	@UI nvarchar(10),
	@FEEEARNER nvarchar(50),
	@CLASSIFICATION nvarchar(20),
	@STARTDATE datetime,
	@ENDDATE datetime',
	@UI,
	@FEEEARNER,
	@CLASSIFICATION,
	@STARTDATE,
	@ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliSrceTop40] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliSrceTop40] TO [OMSAdminRole]
    AS [dbo];

