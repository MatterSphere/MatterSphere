

CREATE PROCEDURE [dbo].[srepFilBilling] 
(
	@UI uUICultureInfo = '{default}',
	@FEEEARNER bigint = null,
	@DEPARTMENT nvarchar(15) = null,
	@FILETYPE nvarchar(15) = null,
	@FUNDTYPE nvarchar(15) = null,
	@LACAT nvarchar(15) = null,
	@BRANCH int = null,
	@STATUS nvarchar(15) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null,
	@ZEROUNBILLED bit
)

as

declare @select nvarchar(4000)
declare @where nvarchar(4000)
declare @orderby nvarchar(100)

--- initilaise the date parms for performance
-- Not required when using UTC format
--if (@STARTDATE is not null)
--begin
--	set @STARTDATE = dbo.GetStartDate(@STARTDATE)
--end
--if (@ENDDATE is not null)
--begin
--	set @ENDDATE = dbo.GetEndDate(@ENDDATE)
--end


--- SET THE SELECT STATEMENT
SET @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
select
	A.Ref,
	A.clname,
	A.fileDesc,
	A.fileprincipleID,
	A.fileDepartment,
	A.fileType,
	A.fileFundCode,
	A.fileLaCategory,
	A.brId,
	A.fileStatus,
	Case
		When D.[Billed So Far] is null then 0
		Else D.[Billed So Far]
	End as [Billed So Far],
	Case
		When B.[Total Unbilled] is null then 0
		Else B.[Total Unbilled]
	End as [Total Unbilled],
	Case
		When C.[Total Billed] is null then 0
		Else C.[Total Billed]
	End as [Total Time Billed],
	D.[Last Billed] as [Last Billed]
from
	(
		SELECT 	
			f.fileID,
			cl.clNo,
			cl.clname,
			f.fileNo,
			f.filedesc ,
			U.usrinits,
			cl.clNo + ''\'' + f.fileNo + '' - '' + U.usrinits as [Ref],
			f.fileprincipleID,
			f.fileDepartment,
			f.fileType,
			f.fileFundCode,
			f.fileLaCategory,
			f.brId,
			f.fileStatus
		from
			dbclient cl 
		inner join
			dbfile f on cl.clid = f.clid
		inner join
			dbuser u on u.usrid = f.fileprincipleid
	) A
LEFT JOIN
	(
		SELECT
			Sum(timecharge) as [Total UnBilled],
			fileID
		FROM
			dbTimeLedger
		WHERE
			timeBilled = 0
		GROUP BY
			FileID
	) B
		ON A.FileID = B.Fileid

LEFT JOIN
	(
		SELECT
			Sum(timecharge) as [Total Billed],
			fileID
		FROM
			dbTimeLedger
		WHERE
			timeBilled = 1
		GROUP BY
			FileID
	) C
		ON A.FileID = C.Fileid
		
LEFT JOIN
	(
		SELECT
			Max(BillDate) as [Last Billed],
			Sum(BillProcosts) as [Billed So Far],
			fileID
		FROM
			dbBillInfo
		GROUP BY
			FileID
	) D
		ON A.FileID = D.FileID '

--- Where Clause
set @where = ''

--- @FEEEARNER filter
if (@FEEEARNER is not null)
begin
	set @where = @where + ' fileprincipleID = @FEEEARNER '
end


--- @DEPARTMENT filter
if (@DEPARTMENT is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' fileDepartment = @DEPARTMENT '
	end
	else
	begin
		set @where = @where + ' AND fileDepartment = @DEPARTMENT '
	end
end

--- @FILETYPE filter
if (@FILETYPE is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' fileType = @FILETYPE '
	end
	else
	begin
		set @where = @where + ' AND fileType = @FILETYPE '
	end
end

--- @FUNDTYPE filter
if (@FUNDTYPE is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' fileFundCode = @FUNDTYPE '
	end
	else
	begin
		set @where = @where + ' AND fileFundCode = @FUNDTYPE '
	end
end

--- @LACAT filter
if (@LACAT is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' fileLACategory = @LACAT '
	end
	else
	begin
		set @where = @where + ' AND fileLACategory = @LACAT '
	end
end

--- @BRANCH filter
if (@BRANCH is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' brID = @BRANCH '
	end
	else
	begin
		set @where = @where + ' AND brID = @BRANCH '
	end
end

--- @STATUS filter
if (@STATUS is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' fileStatus = @STATUS '
	end
	else
	begin
		set @where = @where + ' AND fileStatus = @STATUS '
	end
end

--- Start/end date filters
if (@STARTDATE is not null)
begin
	if (@where = '')
	begin
		set @where = @where + ' ([Last Billed] >= @STARTDATE AND [Last Billed] < @ENDDATE) '
	end
	else
	begin
		set @where = @where + ' AND ([Last Billed] >= @STARTDATE AND [Last Billed] < @ENDDATE) '
	end
end

--- @ZEROUNBILLED clause
if (@ZEROUNBILLED = 1)
begin
	if (@where = '')
	begin
		set @where = @where + ' ([Total Unbilled] is not null AND [Total Unbilled] <> 0) '
	end
	else
	begin
		set @where = @where + ' AND ([Total Unbilled] is not null AND [Total Unbilled] <> 0) '
	end
end


--- Finalise the @where clause
IF (@where <> '')
BEGIN
	SET @where = N' WHERE ' + @where
END


--- ORDER BY clause
set @orderby = N'
order by
	A.Ref '


DECLARE @sql nvarchar(4000)
--- ADD THE CLAUSES TOGETHER
SET @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@orderby)


--- DEBUG PRINT
---PRINT @sql

EXEC sp_executesql @sql, 
N'
	@UI nvarchar(10)
	, @FEEEARNER bigint
	, @DEPARTMENT nvarchar(15)
	, @FILETYPE nvarchar(15)
	, @FUNDTYPE nvarchar(15)
	, @LACAT nvarchar(15)
	, @BRANCH int
	, @STATUS nvarchar(15)
	, @STARTDATE DateTime
	, @ENDDATE DateTime
	, @ZEROUNBILLED bit'
	, @UI
	, @FEEEARNER
	, @DEPARTMENT
	, @FILETYPE
	, @FUNDTYPE
	, @LACAT
	, @BRANCH
	, @STATUS
	, @STARTDATE
	, @ENDDATE
	, @ZEROUNBILLED

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilBilling] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilBilling] TO [OMSAdminRole]
    AS [dbo];

