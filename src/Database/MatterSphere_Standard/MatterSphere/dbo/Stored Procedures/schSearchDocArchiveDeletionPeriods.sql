
CREATE PROCEDURE [dbo].[schSearchDocArchiveDeletionPeriods]
(
	@UI nvarchar(15)
	,@FILETYPE nvarchar(15)
	,@BRID int
	,@CLEARED bit = null
)

AS

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)

set @select = N'SELECT
	DP.ID
	,FT.cdDesc as FileType
	,B.brName as Branch
	,DP.deletionPeriod
FROM
	dbDocumentArchiveDeletionPeriod DP
LEFT JOIN 
	dbo.GetCodeLookupDescription(''FILETYPE'', @UI) FT ON FT.cdCode = DP.FileType	
INNER JOIN 
	dbBranch b on b.brid = DP.brid '		


set @where = N'WHERE B.brID = ISNULL(@BRID, B.brID)
AND FT.cdCode = ISNULL(@FILETYPE, FT.cdCode) '

if @CLEARED =1 
	set @where = @Where +  N'AND  DP.deletionPeriod = -1	'
ELSE IF @CLEARED = 0
	set @where = @Where + N'AND  DP.deletionPeriod != -1 '


declare @sql nvarchar(4000)
set @sql = @select + @where + ' ORDER BY FT.cdDesc, B.brName' 

exec sp_executesql @sql,  N'
	@UI nvarchar(15)
	,@FILETYPE nvarchar(15)
	,@BRID int
	,@CLEARED bit ',
	@UI,
	@FILETYPE,
	@BRID,
	@CLEARED



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocArchiveDeletionPeriods] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocArchiveDeletionPeriods] TO [OMSAdminRole]
    AS [dbo];

