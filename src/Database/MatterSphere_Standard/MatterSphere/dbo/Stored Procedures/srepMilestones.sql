

CREATE PROCEDURE [dbo].[srepMilestones]
(
	@UI uUICultureInfo='{default}',
	@msplan nvarchar(20) = null,
	@FileType nvarchar(20) = null,
	@FileDepartment nvarchar(20) = null,
	@FundingType nvarchar(20) = null,
	@FileStatus nvarchar(20) = null,
	@feePrincipleID int = null
)

AS 

declare @Select nvarchar(4000)
declare @Where nvarchar(1000)

--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	CL.clNo + ''/'' + F.fileNo as Ref,
	Cl.clName,
	F.fileDesc,
	UF.usrFullName as FeeEarner,
	M.SubCode as Stage,
	MSStageDesc,
	M.StageDue,
	M.StageAchieved,
	U.usrFullName as AchievedBy
FROM
	dbo.vwMilestones M
JOIN
	dbo.dbFile F ON F.fileID = M.fileID
JOIN
	dbo.dbClient CL ON CL.clID = F.clID
JOIN
	dbo.dbUser UF ON F.filePrincipleID = UF.usrID
LEFT OUTER JOIN
	dbo.dbUser U ON U.usrID = M.AchievedBy'

---- Debug Print
-- PRINT @SELECT

---- Build Where Clause
SET @WHERE = ' WHERE M.MSStageDesc IS NOT NULL AND M.MSStageDesc <> '''''
--SET @WHERE = ' WHERE Len(M.MSStageDesc) > 0'

-- Milestone plan filter
if (@msplan is not null)
BEGIN
     set @where = @where + ' AND M.MSCode = @msplan'
END

-- filetype filter
if (@filetype is not null)
BEGIN
	set @where = @where + ' AND F.Filetype = @filetype'
END

-- File Dept 
if (@filedepartment is not null)
BEGIN
	set @where = @where + ' AND F.Filedepartment = @filedepartment'
END

-- File Status
if (@FileStatus is not null)
BEGIN
	set @where = @where + ' AND F.filestatus = @FileStatus'
END

-- Funding Type
if (@FundingType is not null)
BEGIN
	set @where = @where + ' AND F.fileFundCode = @FundingType'
END  

-- File Principle
if (@FeePrincipleID is not null)
BEGIN
	set @where = @where + ' AND F.filePrincipleID = @FeePrincipleID'
END  


declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where

--- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	, @msplan nvarchar(20)
	, @FileType nvarchar(20)
	, @FileDepartment nvarchar(20)
	, @FundingType nvarchar(20)
	, @FileStatus nvarchar(20)
	, @feePrincipleID int'
	, @UI
	, @msplan
	, @FileType
	, @FileDepartment
	, @FundingType
	, @FileStatus
	, @feePrincipleID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestones] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestones] TO [OMSAdminRole]
    AS [dbo];

