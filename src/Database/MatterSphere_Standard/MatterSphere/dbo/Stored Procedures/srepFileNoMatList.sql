

CREATE PROCEDURE [dbo].[srepFileNoMatList]
(
	@UI uUICultureInfo='{default}',
	@FileType nvarchar(50) = null,
	@FileDepartment nvarchar(50) = null,
	@brID nvarchar(50)= null,
	@FundingType nvarchar(50) = null,
	@LACategory nvarchar(50) = null,
	@feePrincipleID int = null,
	@MatterStat nvarchar(50) = null,
	@begindate datetime=null,
	@enddate datetime=null
)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)

--- Select Statement for the base Query
set @select = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
SELECT
	C.clNo,
	C.clName,
	F.fileNo,
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc,
	F.fileStatus,
	F.fileDepartment,
	F.fileType, 
    LA.legAidDesc,
	U.usrInits,
	U.usrFullName,
	UFN.ufnCode as [ufnHeadCode],
	F.filePrincipleID,
	F.fileFundCode,
	X.[cdDesc] as DepDesc,
    F.brID,
	F.Created,
	Y.[cdDesc] as fileTypeDesc
FROM
	dbo.dbClient C
INNER JOIN
	dbo.dbFile F ON C.clID = F.clID
INNER JOIN
	dbo.dbFileLegal FL ON F.fileID = FL.fileID
LEFT OUTER JOIN
	dbo.dbLegalAidCategory LA ON F.fileLACategory = LA.legAidCategory
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
INNER JOIN
    dbo.dbUfn UFN ON FL.MatLAUFN = UFN.UFNCode
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DEPT'' , @UI ) X ON X.[cdCode] = F.[fileDepartment]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''FILETYPE'' , @UI ) Y ON Y.[cdCode] = F.[fileType] '

---- Debug Print
-- PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if (@filetype is not null)
BEGIN
	SET @where = @where + ' F.Filetype = @filetype '
END

if (@filedepartment is not null)
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND F.Filedepartment = @filedepartment '
	END
   ELSE
	BEGIN
	     set @where = @where + ' F.FileDepartment = @filedepartment '
	END
END

if (@brID is not null)
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.brID = @brID '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.brID = @brID '
	END
END

if (@FundingType is not null)
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.fileFundCode = @FundingType '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.fileFundCode = @FundingType '
	END
END  

if (@LAcategory is not null)
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.fileLACategory = @lacategory '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.fileLACategory = @lacategory '
	END
END 

if (@FeePrincipleID is not null)
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

if (@MatterStat is not null)
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND F.fileStatus = @MatterStat '
	END
   ELSE
	BEGIN
	    set @where = @where + ' F.filestatus = @MatterStat '
	END
END  

--print @BEGINDATE
--Print @ENDDATE

if (@BEGINDATE is not null)
BEGIN
    IF @where <> ''
	BEGIN
	    set @where = @where + ' AND (F.created >= @BEGINDATE AND F.Created < @ENDDATE) '	
	END
	ELSE
	BEGIN
	    set @where = @where + ' (F.created >= @BEGINDATE AND F.Created < @ENDDATE) '
	END
END

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = Rtrim(@select) + Rtrim(@where)

-- Debug Print
-- print @sql


exec sp_executesql @sql, N'
	@UI nvarchar(10),
	@FileType nvarchar(50),
	@FileDepartment nvarchar(50),
	@brID nvarchar(50),
	@FundingType nvarchar(50),
	@LACategory nvarchar(50),
	@feePrincipleID int,
	@MatterStat nvarchar(50),
	@begindate datetime,
	@enddate datetime',
	@UI,
	@FileType,
	@FileDepartment,
	@brID,
	@FundingType,
	@LACategory,
	@feePrincipleID,
	@MatterStat,
	@begindate,
	@enddate

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileNoMatList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFileNoMatList] TO [OMSAdminRole]
    AS [dbo];

