

CREATE PROCEDURE [dbo].[srepBillBook]
(
	@UI uUICultureInfo='{default}',
	@BillType nvarchar(20) = null,
	@FeeEarner int = null,
	@ClNo nvarchar(20) = null,
	@FileNo nvarchar(20) = null,
	@BillNo nvarchar(10) = null,
	@startdate datetime = null,
	@enddate datetime = null
)

AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	U.usrInits AS FeeInits, 
	U.usrFullName, 
	CL.clNo, 
	CL.clName, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	F.filePrincipleID, 
	BI.billNo, 
	BI.billDate, 
	BI.billCategory, 
	BI.billPaidDisb, 
    BI.billNYPDisb, 
	BI.billVAT, 
	BI.billProCosts
FROM    
	dbo.dbFile F
INNER JOIN
    dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
    dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
    dbo.dbBillInfo BI ON F.fileID = BI.fileID '

--- Build Where Clause
SET @WHERE = ''

-- BillType filter
if (@BillType IS NOT NULL)
BEGIN
	 set @where = @where + ' BI.billcategory = @BillType '
END

-- Client Number filter
if (@ClNo IS NOT NULL AND @ClNo <> '')
BEGIN
	IF (@where <> '' )
	BEGIN
	     set @where = @where + ' AND CL.clno = @ClNo '
	END
	ELSE
	BEGIN
	     set @where = @where + ' CL.clno = @ClNo '
	END
END

-- File No filter
if (@FileNo IS NOT NULL AND @FileNo <> '')
BEGIN
	IF (@where <> '' )
	BEGIN
	     set @where = @where + ' AND F.fileno = @FileNo '
	END
	ELSE
	BEGIN
	     set @where = @where + ' F.fileno = @FileNo '
	END
END

-- Bill Number filter
if (@BillNo IS NOT NULL AND @BillNo <> '')
BEGIN
	IF (@where <> '' )
	BEGIN
	     set @where = @where + ' AND BI.billNo = @BillNo '
	END
	ELSE
	BEGIN
	     set @where = @where + ' BI.billNo = @BillNo '
	END
END

-- Fee Earner filter
if (@FeeEarner IS NOT NULL AND @FeeEarner <> '')
BEGIN
	IF (@where <> '')
	BEGIN
	    set @where = @where + ' AND F.filePrincipleID = @FeeEarner '
	END
	ELSE
	BEGIN
	    set @where = @where + ' F.filePrincipleID = @FeeEarner '
	END
END  

-- StartDate/Enddate filter
if (@startdate IS NOT NULL)
BEGIN
    IF (@where <> '')
	BEGIN
	    set @where = @where + '  AND (BI.billdate >= @startdate AND BI.billdate < @enddate) '	
	END
	ELSE
	BEGIN
		set @where = @where + ' (BI.billdate >= @startdate AND BI.billdate < @enddate) '
	END
END

--- Add Where Clause
if (@where <> '')
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N' ORDER BY BI.billdate ASC'

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = Rtrim(@select) + Rtrim(@where) + Rtrim(@orderby)

--- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	, @BillType nvarchar(20)
	, @FeeEarner int
	, @ClNo nvarchar(20)
	, @FileNo nvarchar(20)
	, @BillNo nvarchar(10)
	, @startdate datetime
	, @enddate datetime'
	, @UI
	, @BillType
	, @FeeEarner
	, @ClNo
	, @FileNo
	, @BillNo
	, @startdate
	, @enddate

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepBillBook] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepBillBook] TO [OMSAdminRole]
    AS [dbo];

