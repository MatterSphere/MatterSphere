

CREATE PROCEDURE [dbo].[srepComScanPost]
(
	@UI uUICultureInfo = '{default}'
	, @FEEEARNER bigint = NULL
	, @DEPT nvarchar(50) = NULL	
	, @BRANCH int = null
	, @STARTDATE datetime = NULL
	, @ENDDATE datetime = NULL
)

AS 

DECLARE @SELECT nVarChar(2000)
DECLARE @WHERE nVarChar(2000)
DECLARE @SQL nVarChar(4000)
DECLARE @ORDERBY nVarChar(200)

-- Start of the select
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
select distinct
	dbo.getfileref(cl.clno, f.fileno) as [Ref],
	cl.clname,
	f.filedesc,
	X.cdDesc as [Department Description],
	Y.cdDesc as [File Type Description],
	Z.cdDesc as [Document Type Description],
	doc.docdesc,
	case
		when doc.docauthored is not null then doc.docauthored
		else doc.created
	end as [Created],
	dbo.getuser(doc.createdby, ''usrfullname'') as [Created By Name],
	v.verlabel as [Version]
from 
	dbdocument doc
inner join
	dbfile f on f.fileid = doc.fileid
inner join
	dbclient cl on cl.clid = f.clid
left outer join
	dbo.GetCodeLookUpDescription(''dept'', @UI) as X on X.cdCode = f.filedepartment
left outer join
	dbo.GetCodeLookUpDescription(''filetype'', @UI) as Y on Y.cdCode = f.filetype
left outer join
	dbo.GetCodeLookUpDescription(''doctype'', @UI) as Z on Z.cdCode = doc.doctype 
left join
	dbDocumentLog dl on dl.docID = doc.docID 
left join
	dbo.dbDocumentVersion v ON v.DocID = doc.DocID '

-- where clause

SET @WHERE = N' where doc.docID <> 0 and coalesce(docdeleted,0) = 0 and dl.logType = ''SCANNED'' '

-- Date ranges
if (@STARTDATE IS NOT NULL)
begin
	set @WHERE = @WHERE + ' and (doc.created >= @STARTDATE and doc.created < @ENDDATE) '
end

-- fee earner
if (@FEEEARNER IS NOT NULL)
begin
	set @where = @where + ' and f.fileprincipleid = @FEEEARNER '
end

-- department
if (@DEPT IS NOT NULL)
begin
	set @where = @where + ' and f.filedepartment = @DEPT '
end

-- branch
if (@BRANCH IS NOT NULL)
begin
	set @where = @where + ' and f.brid = @BRANCH '
end

-- order by clause
set @ORDERBY = N'
order by
	[Ref] '

-- combine into 1 query
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--print @sql

EXEC sp_executesql @SQL, 
N'
	@UI nvarchar(10)
	, @FEEEARNER bigint
	, @DEPT nvarchar(50)
	, @BRANCH int
	, @STARTDATE datetime
	, @ENDDATE datetime'
	, @UI
	, @FEEEARNER
	, @DEPT
	, @BRANCH
	, @STARTDATE
	, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepComScanPost] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepComScanPost] TO [OMSAdminRole]
    AS [dbo];

