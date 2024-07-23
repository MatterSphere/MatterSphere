

CREATE PROCEDURE [dbo].[srepXmasCardList]
(
	@UI uUICultureInfo='{default}',
	@ClType nvarchar(50) = null,
	@XMAS bit = null,
	@FEEEARNER nvarchar(50) = null
)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	U.usrFullName,
	CL.clNo,
	CL.clName,
	CL.cltypeCode,
	X.cdDesc AS ClTypeDesc,
	CL.clPreClient,
	BR.brName, 
    BR.brCode,
	C.contXMASCard
FROM
	dbo.dbClient CL
INNER JOIN
	dbo.dbBranch BR ON CL.brID = BR.brID
INNER JOIN
	dbo.dbContact C ON CL.clDefaultContact = C.contID
INNER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''CLTYPE'', @UI) AS X ON X.cdCode = CL.clTYpeCode '

---- Build Where Clause
SET @WHERE = ''

-- cltype filter
if (@cltype IS NOT NULL)
BEGIN
     set @where = @where + ' CL.cltypecode = @cltype '
END

-- xmas filter
if (@XMAS IS NOT NULL)
BEGIN
	IF (@where <> '')
	BEGIN
	     set @where = @where + ' AND C.contXmasCard = @XMAS '
	END
    ELSE
	BEGIN
	     set @where = @where + ' C.contXmasCard = @XMAS '
	END
END

-- Fee Earner filter
if (@FEEEARNER IS NOT NULL)
BEGIN
	IF (@where <> '')
	BEGIN
	     set @where = @where + ' AND CL.feeUsrID = @FEEEARNER '
	END
    ELSE
	BEGIN
	     set @where = @where + ' CL.feeUsrID = @FEEEARNER '
	END
END

--- Add Where Clause
if (@where <> '')
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N' ORDER BY U.usrFullName '

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@ORDERBY)

-- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	, @ClType nvarchar(50)
	, @XMAS bit
	, @FEEEARNER nvarchar(50)'
	, @UI
	, @ClType
	, @XMAS
	, @FEEEARNER

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepXmasCardList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepXmasCardList] TO [OMSAdminRole]
    AS [dbo];

