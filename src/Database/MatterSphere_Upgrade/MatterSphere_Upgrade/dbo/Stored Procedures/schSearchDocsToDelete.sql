CREATE PROCEDURE [dbo].[schSearchDocsToDelete]
(	@UI uUICultureInfo = '{default}',
	@MAX_RECORDS int = 50,
	@Requestedby int = null,
	@FILETYPE uCodeLookup = null,
	@BRID int
)
AS

declare @Select nvarchar(1900)
declare @Top nvarchar(12)
declare @Where nvarchar(2000)
declare @exclusionfilestatuslist nvarchar(4000)
declare @delimiter nvarchar(1)
declare @action nvarchar(1)

SET @action = 'D'
SET @delimiter = ','

select
	@exclusionfilestatuslist = cast(brxml as XML).value('(/config/settings/property[@name = "DocumentArchivingExclusionFileStatuses"]/@value)[1]', 'nvarchar(4000)')
from
	dbBranch
where
	brID = (select top 1 brID from dbRegInfo)

print @exclusionfilestatuslist
print @delimiter

set @select = N'declare @today date
set @today = CONVERT(DATE, GETDATE())
'

if not @exclusionfilestatuslist is null
	set @select = @select + N'declare @excluded table (fileStatus nvarchar(15) primary key)
insert into @excluded
select items from dbo.SplitStringToTable(@exclusionfilestatuslist, @delimiter)
'

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''

delete from dbDocumentArchiveTempList where Requestedby = @Requestedby


set @select = @select + N'
INSERT INTO dbDocumentArchiveTempList
SELECT DISTINCT ' + @Top + '
	COALESCE(CL1.cdDesc, ''~'' + NULLIF(d.docType, '''') + ''~'') AS doctypedesc,
	d.docID,
	d.docDesc,
	d.docType,
	d.Updated,
	d.UpdatedBy,
	d.Created ,
	CASE ISNULL(exc.DocID , ''-9999'')
		WHEN -9999 THEN 7
		ELSE 6
	END As ImageColumn,
	CASE ISNULL(exc.DocID, ''0'')
		WHEN 0 THEN 0
		ELSE 1
	END As Excluded,
	cl.clNo,
	f.fileID,
	f.fileNo,
	dbo.GetFileRef(cl.clNo, f.fileNo) as ClientFileNo,
	cl.clName,
	f.fileDesc,
	d.CreatedBy,
	' + Convert(nvarchar, @Requestedby) + ' ,
	d.docdirID,
	@action,
	null
FROM
	dbDocument d
INNER JOIN
	dbFile f ON d.fileID = f.fileID
INNER JOIN
	dbClient cl ON f.clID = cl.clID
--INNER JOIN
--	dbUser u on u.usrid = d.CreatedBy
--INNER JOIN
--	dbBranch b on b.brid = f.brid
LEFT JOIN
	dbDocumentArchiveExclusion exc ON d.docID = exc.DocID AND exc.ArchiveAction = @action
LEFT JOIN
	dbDocumentArchiveDeletionPeriod DP ON DP.brID = f.brID AND DP.fileType = f.fileType
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL1 ON CL1.[cdCode] = d.docType
'


set @where = N'WHERE d.docID <> 0 AND d.docdirID is not null AND (f.brid = @BRID OR (@BRID IS NULL AND f.brid IS NOT NULL)) AND (f.fileType = @FILETYPE OR @FILETYPE IS NULL)'

set @where = @where + N' AND datediff (day, convert(date, f.fileClosed), @today) >= DP.deletionPeriod AND DP.deletionPeriod != -1'

if not @exclusionfilestatuslist is null
BEGIN
	set @where = @where + N' AND f.FileStatus not in (select fileStatus from @excluded)'
END

set @where = @where + N' AND d.docID NOT IN (select ArchDocID as docID from dbDocumentArchiveInfo where archstatus = 1)' -- *** Ignore documents that are waiting to be archived/deleted - Archive Status = 1.

set @where = @where + N' AND d.docID NOT IN (select docID from dbDocumentArchiveTempList)'

set @where = @where + N' AND d.docFlags & 1 = 0'


declare @sql nvarchar(4000)

set @sql = @select + @where + ' ORDER BY d.Created ASC'	-- *** might not need this

print @sql



exec  sp_executesql @sql, N'
	@UI uUICultureInfo,
	@MAX_RECORDS int,
	@exclusionfilestatuslist nvarchar(4000),
	@delimiter nvarchar(1),
	@Requestedby int,
	@action nvarchar(1),
	@FILETYPE uCodeLookup,
	@BRID int',
	@UI,
	@MAX_RECORDS,
	@exclusionfilestatuslist,
	@delimiter,
	@Requestedby,
	@action,
	@FILETYPE,
	@BRID

	select * from dbDocumentArchiveTempList
	where Requestedby = @Requestedby and ArchiveAction = @action


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocsToDelete] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocsToDelete] TO [OMSAdminRole]
    AS [dbo];

