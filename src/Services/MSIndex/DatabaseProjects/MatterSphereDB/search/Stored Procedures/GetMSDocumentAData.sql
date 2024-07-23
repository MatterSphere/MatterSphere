CREATE PROCEDURE search.GetMSDocumentAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Document'
	, @X XML
	, @docID BIGINT
	, @TABLENAME NVARCHAR(100) = 'config.dbDocument'

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

SELECT 
	@BULKREQUIRED = CASE WHEN cvc.FullCopyRequired = 1 OR IE.FullCopyRequired = 1 THEN 1 ELSE 0 END
	, @COPIEDVERSION = LastCopiedVersion
	, @WORKINGVERSION = WorkingVersion
FROM search.IndexedEntity IE
	CROSS APPLY search.ChangeVersionControl  CVC
WHERE IE.EntityName = @EntityName

IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
	SET @BULKREQUIRED = 1

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize) 
		d.docID
		, 'I'
	FROM config.dbDocument d 
		INNER JOIN dbo.dbDirectory dir  on dir.dirID = d.docdirID
	ORDER BY d.docID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT
		r.docId
		, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
	FROM(
	SELECT CT.docID
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbDocument, @COPIEDVERSION) AS CT
	UNION
	SELECT ISNULL(UserGroup_Document.DocumentID, Usergroup_Document_Delete.DocumentID) 
		, 'U'
	FROM CHANGETABLE( CHANGES relationship.UserGroup_Document, @COPIEDVERSION) AS CT
		LEFT OUTER JOIN relationship.UserGroup_Document ON CT.RelationshipID = UserGroup_Document.RelationshipID
		LEFT OUTER JOIN relationship.Usergroup_Document_Delete ON CT.RelationshipID = Usergroup_Document_Delete.ID
	UNION
	SELECT dbDocument.docID 
		, 'U'
	FROM CHANGETABLE( CHANGES [dbo].[dbCodeLookup], @COPIEDVERSION) AS CT
		INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
		INNER JOIN config.dbDocument ON dbDocument.doctype = dbCodeLookup.cdCode
	WHERE dbCodeLookup.cdType = 'DOCTYPE'
	UNION
	SELECT d.docId
		, 'U' 
	FROM
	(
		SELECT 
			ISNULL(UserGroup_file.fileID, Usergroup_File_Delete.fileID) AS fileID
		FROM CHANGETABLE( CHANGES relationship.UserGroup_file, @COPIEDVERSION) AS CT
			LEFT JOIN relationship.UserGroup_file ON CT.RelationshipID = UserGroup_file.RelationshipID
			LEFT JOIN relationship.Usergroup_File_Delete ON CT.RelationshipID = Usergroup_File_Delete.ID
	) f
	INNER JOIN config.dbDocument d ON f.fileID = d.fileID
	) r
		INNER JOIN config.dbDocument AS d ON d.docId = r.docID
	GROUP BY r.docId

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END

SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @docID IS NOT NULL
BEGIN

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT base.docID AS [mattersphereid]
			, base.AssocID AS [associate-id]
			, base.clID as [client-id]
			, base.fileID as [file-id]
			, base.docDeleted AS [docDeleted]
			, LOWER(REPLACE(base.docExtension,'.','')) AS [documentExtension]
			, COALESCE(cl1.cdDesc, '~' + base.docType + '~') AS [documentType]
			, dbo.RemoveIllegalXMLCharacters(base.docDesc) AS [docDesc]
			, auth.usrFullName AS [usrFullName]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM config.dbDocument base 
			INNER JOIN dbo.dbDirectory dir ON dir.dirID = base.docdirID
			LEFT OUTER JOIN dbo.dbUser auth ON base.docAuthoredBy = auth.usrid
			LEFT JOIN dbo.GetCodeLookupDescription('DOCTYPE', @UI) cl1 ON cl1.cdCode = base.docType
			LEFT JOIN search.FormatMSPermForAS(@EntityName, @T1) obj ON obj.ID = base.docID
			INNER JOIN @TBatch tb ON tb.Id = base.docID
		FOR XML RAW
		)
	
	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch
		SELECT TOP (@BatchSize)
			d.docID
			, 'I'
		FROM config.dbDocument d 
			INNER JOIN dbo.dbDirectory dir ON dir.dirID = d.docdirID
		WHERE d.docID > @docId
		ORDER BY d.docID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @docId
		ORDER BY Id

	SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
