

CREATE PROCEDURE [dbo].[srepWIPbyFiles]
(@UI uUICultureInfo='{default}',
@FileType nvarchar(50) = null,
@FileDepartment nvarchar(50) = null,
@brID nvarchar(50)= null,
@FundingType nvarchar(50) = null,
@LACategory nvarchar(50) = null,
@feePrincipleID int = null,
@MatterStat nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

declare @Select nvarchar(max)
declare @Where nvarchar(2000)


--- Select Statement for the base Query
set @select = N'
SELECT  A.clName , A.clNo , A.fileDesc , A.fileStatus , A.fileDepartment , A.fileTypeDesc , fileType , A.FeeInits , A.fileNo , A.fileLACategory , A.brID , A.fileFundCode , A.Created , A.usrFullName , B.SumTimeCharge  FROM
	(
	SELECT     
		dbo.dbClient.clName, 
		replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, 
		dbo.dbFile.fileStatus, 
		dbo.dbFile.fileDepartment, 
		COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbo.dbFile.fileType, '''') + ''~'') AS FileTypeDesc, 
		dbo.dbFile.fileType, 
		dbo.dbUser.usrInits AS FeeInits, 
		dbo.dbFile.fileNo, 
		dbo.dbClient.clNo, 
		dbo.dbFile.fileLACategory, 
		dbo.dbFile.brID, 
	        dbo.dbFile.fileFundCode, 
		dbo.dbFile.Created, 
	        dbo.dbUser.usrFullName, 
		dbo.dbfile.fileID,
		dbo.dbfile.filePrincipleID
	FROM         
		dbo.dbClient 
	INNER JOIN
		dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID 
	INNER JOIN
		dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID
	LEFT JOIN 
		dbo.GetCodeLookupDescription ( ''FILETYPE'' , @UI ) CL1 ON CL1.cdCode = dbo.dbFile.fileType
	) AS A
JOIN
	(
	SELECT fileID , sum ( timeCharge ) as SumTimeCharge from dbTimeLedger GROUP BY fileID
	) AS B ON A.fileID = B.fileID '

---- Debug Print
--PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

if coalesce(@filetype, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND A.Filetype =  @filetype '
	END
   ELSE
	BEGIN
	     set @where = @where + ' A.Filetype =  @filetype '
	END
END

if coalesce(@filedepartment, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND A.Filedepartment = @filedepartment '
	END
   ELSE
	BEGIN
	     set @where = @where + ' A.FileDepartment = @filedepartment '
	END
END

if coalesce(@brID, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.brID = @brID '
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.brID = @brID '
	END
END

if coalesce(@FundingType, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.fileFundCode = @FundingType '
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.fileFundCode = @FundingType '
	END
END  

if coalesce(@LAcategory, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.fileLACategory = @lacategory '
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.fileLACategory = @lacategory '
	END
END 

if not @FeePrincipleID is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.filePrincipleID = @FeePrincipleID '
	END
   ELSE
	BEGIN
	   set @where = @where + ' A.filePrincipleID = @FeePrincipleID '
	END
END  

if coalesce(@MatterStat, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND A.filestatus = @Matterstat '
	END
   ELSE
	BEGIN
	    set @where = @where + ' A.filestatus = @MatterStat '
	END
END 

print @BEGINDATE
Print @ENDDATE

if coalesce(@BEGINDATE, '') <> ''
BEGIN
   IF @where = '' or @where is null
	BEGIN
	    if @enddate is null
		BEGIN
		    set @where = @where + ' A.Created BETWEEN @BEGINDATE AND @BEGINDATE'	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' A.Created BETWEEN @BEGINDATE AND @ENDDATE '
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND A.Created BETWEEN @BEGINDATE AND @BEGINDATE '	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND A.Created BETWEEN @BEGINDATE AND @ENDDATE'
		END
	END
END  

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END


declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where --+ @orderby 

--- Debug Print
-- print @sql


exec sp_executesql @sql, N'@UI nvarchar(10) , @fileType nvarchar(50) , @fileDepartment nvarchar(50) , @brID nvarchar(50) , @fundingType nvarchar(50) , @LACategory nvarchar(50) , @feePrincipleID int , @MatterStat nvarchar(50) ,
							@BeginDate datetime , @EndDate datetime ' , @UI , @fileType , @fileDepartment , @brID ,@fundingType , @LACategory , @feePrincipleID , @MatterStat , @BeginDate , @EndDate

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepWIPbyFiles] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepWIPbyFiles] TO [OMSAdminRole]
    AS [dbo];

