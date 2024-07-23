

CREATE PROCEDURE [dbo].[srepFilInactivityDoc]
(
	@UI uUICultureInfo='{default}',
	@INACTIVITY int = 30,
	@FILETYPE nvarchar(50) = null,
	@DEPARTMENT nvarchar(50) = null,
	@FUNDTYPE nvarchar(50) = null,
	@LACAT nvarchar(50) = null,
	@FEEEARNER bigint = null,
	@FILESTATUS nvarchar(50) = null
)

AS 

create table #DocInactivity (
	fileid bigint not null
	, created datetime
)

insert into #DocInactivity
	(
		fileid
		, created
	)
select
	f.fileid
	, max(d.created) as MaxCreatedDate
from 
	dbFile F 
with 
	(nolock)

inner join
	dbDocument D on f.fileid = D.fileid
where 
	F.fileType = COALESCE(@FILETYPE, F.fileType) AND
	F.fileDepartment = coalesce(@DEPARTMENT, F.fileDepartment) AND
	F.fileFundCode = COALESCE(@FUNDTYPE, F.fileFundCode) AND
	F.fileStatus = COALESCE(@FILESTATUS, F.fileStatus) AND
	coalesce(F.fileLACategory,'') = COALESCE(@LACAT, F.fileLACategory,'') AND --lacat can be null so coalesce both sides
	F.filePrincipleID = COALESCE(@FEEEARNER, F.filePrincipleID)
group by 
	f.fileid
having
	max(d.created) < getutcdate()-@inactivity

SELECT    
	U.usrInits, 
	U.usrFullName, 
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), ', ') as fileDesc, 
	COALESCE(CL1.cdDesc, '~' + NULLIF(F.fileFundCode, '') + '~') AS FundTypeDesc, 
	F.fileLACategory, 
	COALESCE(CL2.cdDesc, '~' + NULLIF(F.fileType, '') + '~') AS FileTypeDesc, 
    COALESCE(CL3.cdDesc, '~' + NULLIF(F.fileDepartment, '') + '~') AS DeptDesc, 
	TMP.Created AS Created, 
	DATEDIFF(d, TMP.Created, getutcdate()) AS DateDiff
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
INNER JOIN
	#DocInactivity TMP ON F.fileID = TMP.fileID
LEFT JOIN dbo.GetCodeLookupDescription ( 'FUNDTYPE', @UI ) CL1 ON CL1.[cdCode] =  F.fileFundCode
LEFT JOIN dbo.GetCodeLookupDescription ( 'FILETYPE', @UI ) CL2 ON CL2.[cdCode] =  F.fileType
LEFT JOIN dbo.GetCodeLookupDescription ( 'DEPT', @UI ) CL3 ON CL3.[cdCode] =  F.fileDepartment
WHERE
	DATEDIFF(d, TMP.Created, getutcdate()) >= COALESCE(@INACTIVITY, DATEDIFF(d, TMP.Created, getutcdate())) AND
	F.fileType = COALESCE(@FILETYPE, F.fileType) AND
	F.fileDepartment = coalesce(@DEPARTMENT, F.fileDepartment) AND
	F.fileFundCode = COALESCE(@FUNDTYPE, F.fileFundCode) AND
	F.fileStatus = COALESCE(@FILESTATUS, F.fileStatus) AND
	coalesce(F.fileLACategory,'') = COALESCE(@LACAT, F.fileLACategory,'') AND --lacat can be null so coalesce both sides
	F.filePrincipleID = COALESCE(@FEEEARNER, F.filePrincipleID)
ORDER BY 
	DATEDIFF(d, TMP.Created, getutcdate()) desc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilInactivityDoc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilInactivityDoc] TO [OMSAdminRole]
    AS [dbo];

