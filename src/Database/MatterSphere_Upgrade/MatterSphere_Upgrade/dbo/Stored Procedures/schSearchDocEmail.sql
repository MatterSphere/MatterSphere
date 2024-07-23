


CREATE PROCEDURE [dbo].[schSearchDocEmail] 
(
	@CURRENTUSER int = null, 
	@UI uUICultureInfo = '{default}', 
	@FILEID bigint = 0,
	@MAX_RECORDS int = 100,  
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
	@AUTHOREDBY int = null
)  
AS

if @CURRENTUSER is null
	set @CURRENTUSER = dbo.GetCurrentUser()

if @FILEID is null set @FILEID = 0

declare @Select nvarchar(2500)
declare @Top nvarchar(10)
declare @Where nvarchar(2000)

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''

set @select = 
	N' 	select DocumentID into #R1 from [config].[searchDocumentAccess] (''FILE'',@FILEID) r
	CREATE UNIQUE CLUSTERED INDEX AF1 ON #R1 ([DocumentID])
	
	SELECT ' + @Top + '
		dbo.dbUser.usrInits AS CrByInits, 
		dbo.dbUser.usrFullName AS CrByFullName, 
		dbUser_1.usrInits AS UpdByInits, 
		dbUser_1.usrFullName AS UpdByFullName, 
		dbUser_2.usrInits AS AuthByInits, 
		dbUser_2.usrFullName AS AuthByFullName, 
		COALESCE(CL.cdDesc, ''~'' + NULLIF(dbDocument.docType, '''') + ''~'') AS doctypedesc, 
		dbDocument.docID, 
		dbDocument.created as crfield,
		dbDocument.docDesc, 
		dbDocument.docType, 
		COALESCE(dbDocument.docAuthored, dbDocument.Created) AS Created, 
		dbDocument.Updated, 
		dbDocument.docDirection, 
		dbDocument.docExtension,
		dbClient.clNo, 
		dbFile.fileNo, 
		dbDocument.docstyledesc, 
		COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbDocument.docwallet, '''') + ''~'') as docwallet,
		CONT.contName,
		convert(nvarchar(20),dbDocument.docid) as docidtxt,
		dbDocument.docFileName as doctoken,
		dbDocument.doccheckedout,
		dbDocument.doccheckedoutby,
		dbDocument.doccheckedoutlocation,
		V.verLabel,
		DE.docFrom,
		DE.docTo,
		DE.docCC,
		DE.docSent,
		DE.docReceived,
		DE.docAttachments
		,dbDocument.DocFlags
FROM         
	dbo.dbUser 
RIGHT OUTER JOIN
					config.dbDocument 
LEFT OUTER JOIN
					config.dbFile ON dbDocument.fileID = dbFile.fileID 
LEFT OUTER JOIN
					config.dbClient ON dbDocument.clID = dbClient.clID 
LEFT OUTER JOIN
					dbo.dbUser dbUser_1 ON dbDocument.UpdatedBy = dbUser_1.usrID ON dbo.dbUser.usrID = dbDocument.Createdby
LEFT OUTER JOIN
					  dbo.dbUser dbUser_2 ON dbDocument.docAuthoredBy = dbUser_2.usrID
INNER JOIN
					dbassociates ASS on ASS.associd = dbDocument.associd
INNER JOIN 
					dbcontact CONT on CONT.contid = ASS.contid
LEFT JOIN 
					dbo.dbApplication A ON A.appID = dbDocument.docAppID
LEFT JOIN
					dbo.dbDocumentVersion V ON v.verID = dbDocument.docCurrentVersion
INNER JOIN
	dbDocumentEmail DE ON DE.docID = dbDocument.docID
JOIN  #R1 r on r.DocumentID = dbdocument.docid
LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL ON CL.[cdCode] = dbDocument.docType
LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''WALLET'', @ui ) CL1 ON CL1.[cdCode] = dbDocument.docwallet
	'

set @where = N' WHERE dbDocument.FILEID = @FILEID and doctype = ''email'''

IF @DESC <> '' and not @DESC is null
	BEGIN 
		set @where = @where + N' AND DocDesc LIKE ''%'' + @DESC + ''%'''
	END

IF not @CREATEDBY is null
	BEGIN 
		set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
	END

if not @DOCTYPE is null
	set @where = @where + N' AND doctype = @DOCTYPE'

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
	SET @WHERE = @WHERE + ' AND dbDocument.docAppID = @APPTYPE'
END

-- Wallet
IF(@WALLET IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbDocument.docWallet = @WALLET'
END

-- Date Created Range
IF(@CREATEDSTART IS NOT NULL)
BEGIN
	BEGIN
		SET @WHERE = @WHERE + ' AND dbDocument.Created >= @CREATEDSTART AND dbDocument.Created < @CREATEDEND '
	END
END

-- Date Updated Range
IF(@UPDATEDSTART IS NOT NULL)
BEGIN
	BEGIN
		SET @WHERE = @WHERE + ' AND dbDocument.Updated >= @UPDATEDSTART AND dbDocument.Updated < @UPDATEDEND'
	END
END

-- Document Updated By
IF(@UPDATEDBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbDocument.UpdatedBy = @UPDATEDBY'
END

-- Date Authored Range
IF(@AUTHOREDSTART IS NOT NULL)
BEGIN
	BEGIN
		SET @WHERE = @WHERE + ' AND dbDocument.docAuthored >= @AUTHOREDSTART AND dbDocument.docAuthored < @AUTHOREDEND'
	END
END

-- Document Authored By
IF(@AUTHOREDBY IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND dbDocument.docAuthoredBy = @AUTHOREDBY'
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
set @sql = @select + @where + 'ORDER BY dbdocument.Created DESC option (HASH GROUP, FAST 50)'
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
	@AUTHOREDBY int', 
	
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
	@AUTHOREDBY



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocEmail] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocEmail] TO [OMSAdminRole]
    AS [dbo];

