

CREATE PROCEDURE [dbo].[srepMSStageCompleted]
(
	@UI uUICultureInfo='{default}',
	@msplan nvarchar(50) = null,
	@msstage nvarchar(50) = null,
	@source nvarchar(50)= null,
	@feePrincipleID int = null,
	@begindate datetime=null,
	@enddate datetime=null,
	@status nvarchar(20) = null
)

AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)

--- Select Statement for the base Query
set @select = N'
SELECT
	dbo.GetFileRef(CL.clNo, F.fileNo) AS ClientFileRef,  
	F.filePrincipleID, 
	F.fileNo, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	CL.clNo, 
	CL.clName, 
	U.usrInits, 
    U.usrFullName, 
	MSD.MSNextDueDate, 
	MSD.MSNextDueStage, 
	MSD.MSCode, 
    MSD.MSCreated
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbMSData_OMS2K MSD ON F.fileID = MSD.fileID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID'

--- Build Where Clause
SET @WHERE = N''

if @msplan IS NOT NULL
BEGIN
	set @where = @where + ' MSD.mscode = @msplan '
END

if @msstage IS NOT NULL
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND (MSD.msnextduestage = @msstage) '
	END
   ELSE
	BEGIN
	     set @where = @where + ' (MSD.msnextduestage =  @msstage) '
	END
END

if (@Source IS NOT NULL)
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND CL.clsource = @source ' 
	END
   ELSE
	BEGIN
	    set @where = @where + ' CL.clsource = @source ' 
	END
END

if @FeePrincipleID is not null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.filePrincipleID = @FeePrincipleID '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.filePrincipleID = @FeePrincipleID '
	END
END  


if @status is not null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.fileStatus = @status '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.fileStatus = @status '
	END
END  

if (@BEGINDATE IS NOT NULL)
BEGIN
    IF @where <> ''
	BEGIN
		set @where = @where + ' AND (MSD.MSNextDueDate >= @BEGINDATE AND MSD.MSNextDueDate < @ENDDATE) '	
	END
	ELSE
	BEGIN
		set @where = @where + '  (MSD.MSNextDueDate >= @BEGINDATE AND MSD.MSNextDueDate < @ENDDATE) '	
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
--set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = Rtrim(@select) + Rtrim(@where)

-- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10)
	,@msplan nvarchar(50)
	,@msstage nvarchar(50)
	,@source nvarchar(50)
	,@feePrincipleID int
	,@begindate datetime
	,@enddate datetime
	,@status nvarchar(20)'
	,@UI
	,@msplan
	,@msstage
	,@source
	,@feePrincipleID
	,@begindate
	,@enddate
	,@status

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMSStageCompleted] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMSStageCompleted] TO [OMSAdminRole]
    AS [dbo];

