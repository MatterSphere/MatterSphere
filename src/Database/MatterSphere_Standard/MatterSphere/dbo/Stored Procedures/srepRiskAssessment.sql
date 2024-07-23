

CREATE PROCEDURE [dbo].[srepRiskAssessment]
(
	@UI uUICultureInfo='{default}',
	@RiskId int = null,
	@Between int ,
	@And int 
)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(1000)
declare @OrderBy nvarchar(400)


--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
select
	cl.clno + ''/'' + f.fileno as ref,
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc,
	cl.clName as Client,
	U.usrFullName as FeeEarner,
	rc.riskDescription,
	rh.riskTotalScore
from
	dbRiskHeader rh
inner join
	dbo.dbRiskConfig RC ON RH.riskCode = RC.RiskCode
inner join
	dbFile F ON F.fileID = RH.fileID
inner join
	dbUser u on f.filePrincipleId = U.usrID
inner join
	dbClient CL on f.clid = cl.clid'

---- Build Where Clause with mandatory filters
SET @WHERE = ' WHERE rh.riskTotalScore BETWEEN @Between AND @And AND rh.riskActive = 1'

-- risk type filter
if (@RiskId IS NOT NULL)
BEGIN
	SET @where = @where + ' AND RC.riskId = @riskId'
END

-- Order By
SET @OrderBY = N' ORDER BY Ref, riskTotalScore'

declare @sql nvarchar(3000)
--- Add Statements together
set @sql = @select + @where + @OrderBy

-- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	, @RiskId int
	, @Between int
	, @And int'
	, @UI
	, @RiskId
	, @Between
	, @And

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepRiskAssessment] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepRiskAssessment] TO [OMSAdminRole]
    AS [dbo];

