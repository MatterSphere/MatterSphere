CREATE PROCEDURE [dbo].[ptlSchSearchDoc] 
(
	@CURRENTUSER int = null, 
	@UI uUICultureInfo = '{default}', 
	@FILEID bigint = 0,
	@MAX_RECORDS int = 50,  
	@DESC nvarchar(100) = '', 
	@CREATEDBY int = null, 
	@DOCTYPE uCodeLookup = null, 
	@IN bit = 1, 
	@OUT bit = 1, 
	@ASSOCID bigint = null, 
	@IncludeDeleted bit = 0, 
	
	-- Parameters added 07/01/2009
	@APPTYPE nvarchar(15) = null, 
	@WALLET nvarchar(15) = null, 
	@CREATEDSTART datetime = null, 
	@CREATEDEND datetime = null, 
	@UPDATEDSTART datetime = null, 
	@UPDATEDEND datetime = null, 
	@UPDATEDBY int = null, 
	@AUTHOREDSTART datetime = null, 
	@AUTHOREDEND datetime = null, 
	@AUTHOREDBY int = null,
	@PAGE_NUM INT = NULL, 
	@FolderGUID uniqueidentifier = null
)
AS

if @CURRENTUSER is null
	set @CURRENTUSER = dbo.GetCurrentUser()

if @FILEID is null set @FILEID = 0

declare @Select nvarchar(max)
declare @Top nvarchar(10)
declare @Where nvarchar(2000)

DECLARE @PAGE_ROW_FROM int
DECLARE @PAGE_ROW_TO int
SET @PAGE_ROW_FROM = ((@PAGE_NUM - 1) * (@MAX_RECORDS) + 1)
SET @PAGE_ROW_TO = (@PAGE_NUM * @MAX_RECORDS)


set @select = 
	N'WITH [Data] AS (SELECT  
		dbo.dbUser.usrInits AS CrByInits, 
		dbo.dbUser.usrFullName AS CrByFullName, 
		dbUser_1.usrInits AS UpdByInits, 
		dbUser_1.usrFullName AS UpdByFullName, 
		dbUser_2.usrInits AS AuthByInits, 
		dbUser_2.usrFullName AS AuthByFullName, 
		COALESCE(CL.cdDesc, ''~'' +  NULLIF(dbDocument.docType, '''') + ''~'') AS doctypedesc, 
		dbo.dbDocument.docID, 
		dbo.dbdocument.created as crfield,
		dbo.dbDocument.docDesc, 
		dbo.dbDocument.docType, 
		COALESCE(dbo.dbDocument.docAuthored, dbo.dbDocument.Created) AS Created, 
		dbo.dbDocument.Updated, 
		dbo.dbDocument.docDirection, 
		dbo.dbDocument.docExtension,
		dbo.dbClient.clNo, 
		dbo.dbFile.fileNo, 
		dbo.dbdocument.docstyledesc, 
		COALESCE(CL1.cdDesc, ''~'' +  NULLIF(dbdocument.docwallet, '''') + ''~'') as docwallet,
		CONT.contName,
		convert(nvarchar(20),dbo.dbDocument.docid) as docidtxt,
		dbo.dbdocument.docFileName as doctoken,
		dbo.dbdocument.doccheckedout,
		dbo.dbdocument.doccheckedoutby,
		dbo.dbdocument.doccheckedoutlocation,
		V.verLabel,
		ROW_NUMBER() OVER (ORDER BY dbo.dbDocument.Updated DESC) AS [RowNum]
FROM 
	dbo.dbUser 
RIGHT OUTER JOIN
					dbo.dbDocument 
LEFT OUTER JOIN
					dbo.dbFile ON dbo.dbDocument.fileID = dbo.dbFile.fileID 
LEFT OUTER JOIN
					dbo.dbClient ON dbo.dbDocument.clID = dbo.dbClient.clID 
LEFT OUTER JOIN
					dbo.dbUser dbUser_1 ON dbo.dbDocument.UpdatedBy = dbUser_1.usrID ON dbo.dbUser.usrID = dbo.dbDocument.Createdby
LEFT OUTER JOIN
					  dbo.dbUser dbUser_2 ON dbo.dbDocument.docAuthoredBy = dbUser_2.usrID
INNER JOIN
					dbassociates ASS on ASS.associd = dbDocument.associd
INNER JOIN 
					dbcontact CONT on CONT.contid = ASS.contid
LEFT JOIN 
					dbo.dbApplication A ON A.appID = dbDocument.docAppID
LEFT JOIN
					dbo.dbDocumentVersion V ON v.verID = dbo.dbDocument.docCurrentVersion
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL ON CL.[cdCode] = dbDocument.docType
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''WALLET'', @ui ) CL1 ON CL1.[cdCode] = dbdocument.docwallet
'

set @where = N' WHERE dbo.dbDocument.FILEID = @FILEID '

IF @FolderGUID is not null
BEGIN
	SET @where = @where + ' AND dbDocument.docFolderGUID = @FolderGUID'
END

IF @DESC <> '' and not @DESC is null
BEGIN 
	set @where = @where + N' AND DocDesc LIKE ''%'' + @DESC + ''%'''
END

IF not @CREATEDBY is null
BEGIN 
	set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
END

IF not @DOCTYPE is null
BEGIN
	set @where = @where + N' AND doctype = @DOCTYPE'
END

IF @IN = 1 and @OUT = 0 
BEGIN
	set @where = @where + N' AND docdirection = 1'
END

IF @IN = 0 and @OUT = 1
BEGIN
	set @where = @where + N' AND docdirection = 0'
END

IF @ASSOCID is not null
BEGIN
	set @where = @where + N' AND dbdocument.AssocID = @AssocID'
END 

--------------------------------------
-- Additional filters applied to WHERE clasue 07/01/2009

-- Application Type of the Document
IF (@APPTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.docAppID = @APPTYPE'
END

-- Wallet
IF(@WALLET IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.docWallet = @WALLET'
END

-- Date Created Range
IF(@CREATEDSTART IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.Created >= @CREATEDSTART AND dbo.dbdocument.Created < @CREATEDEND '
END

-- Date Updated Range
IF(@UPDATEDSTART IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.Updated >= @UPDATEDSTART AND dbo.dbdocument.Updated < @UPDATEDEND'
END

-- Document Updated By
IF(@UPDATEDBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.UpdatedBy = @UPDATEDBY'
END

-- Date Authored Range
IF(@AUTHOREDSTART IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.docAuthored >= @AUTHOREDSTART AND dbo.dbdocument.docAuthored < @AUTHOREDEND'
END

-- Document Authored By
IF(@AUTHOREDBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbo.dbDocument.docAuthoredBy = @AUTHOREDBY'
END
--------------------------------------



-- New code
IF EXISTS ( SELECT C.[name] FROM syscolumns C JOIN sysobjects O ON C.[id] = O.[id] WHERE O.[name] = 'dbDocument'  and C.[name] = 'docdeleted' )
BEGIN
	if @IncludeDeleted = 0
	begin
		set @where = @where + N' and coalesce(docdeleted,0) = 0'
	end
END


declare @sql nvarchar(4000)
set @sql = @select + @where + ') SELECT * FROM  [Data] ' 
if (@PAGE_ROW_FROM is not null AND @PAGE_ROW_TO is not null)
BEGIN
set @sql = @sql + ' WHERE [RowNum]  BETWEEN @PAGE_ROW_FROM AND @PAGE_ROW_TO'
END
SET @sql = @sql + ' ORDER BY Updated DESC' -- ' ORDER BY dbo.dbdocument.Created DESC'
print @sql

exec sp_executesql @sql,  N'
	@CURRENTUSER int, 
	@UI uUICultureInfo, 
	@FILEID bigint,
	@MAX_RECORDS int,  
	@DESC nvarchar(100), 
	@CREATEDBY int, 
	@DOCTYPE uCodeLookup, 
	@IN bit, 
	@OUT bit, 
	@ASSOCID bigint,

	@APPTYPE nvarchar(15), 
	@WALLET nvarchar(15), 
	@CREATEDSTART datetime, 
	@CREATEDEND datetime, 
	@UPDATEDSTART datetime, 
	@UPDATEDEND datetime, 
	@UPDATEDBY int, 
	@AUTHOREDSTART datetime, 
	@AUTHOREDEND datetime, 
	@AUTHOREDBY int,
	@PAGE_ROW_FROM int,
	@PAGE_ROW_TO int,
	@FolderGUID uniqueidentifier',
	
	@CURRENTUSER, 
	@UI, 
	@FILEID, 
	@MAX_RECORDS, 
	@DESC, 
	@CREATEDBY,
	@DOCTYPE,
	@IN,
	@OUT,
	@ASSOCID,

	-- Parameters added 07/01/2009
	@APPTYPE, 
	@WALLET, 
	@CREATEDSTART, 
	@CREATEDEND, 
	@UPDATEDSTART, 
	@UPDATEDEND, 
	@UPDATEDBY, 
	@AUTHOREDSTART, 
	@AUTHOREDEND, 
	@AUTHOREDBY,
	@PAGE_ROW_FROM,
	@PAGE_ROW_TO,
	@FolderGUID
