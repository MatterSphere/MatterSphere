CREATE PROCEDURE search.GetMSFileAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'File'
	, @X XML
	, @fileID BIGINT
	, @TABLENAME NVARCHAR(100) = 'config.dbfile'

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
		fileID
		, 'I'
	FROM config.dbFile
	ORDER BY fileID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT
		fileID
		, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
	FROM (
		SELECT 
			fileID 
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE( CHANGES config.dbfile, @COPIEDVERSION) AS CT
		UNION
		SELECT 
			ISNULL(UserGroup_file.fileID, Usergroup_File_Delete.fileID)
			, 'U'
		FROM CHANGETABLE( CHANGES relationship.UserGroup_file, @COPIEDVERSION) AS CT
			LEFT JOIN relationship.UserGroup_file ON CT.RelationshipID = UserGroup_file.RelationshipID
			LEFT JOIN relationship.Usergroup_File_Delete ON CT.RelationshipID = Usergroup_File_Delete.ID
		UNION
		SELECT 
			dbfile.fileID
			, 'U' 
		FROM CHANGETABLE( CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
			INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
			INNER JOIN config.dbfile ON ((dbfile.filetype = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'FileType') OR (dbfile.fileStatus = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'FileStatus'))
		) r
	GROUP BY r.fileID

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END

SET @fileID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @fileID IS NOT NULL
BEGIN

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT base.fileID AS [mattersphereid]
			, base.clID AS [client-id]
			, COALESCE(cl1.cdDesc, '~' + base.fileType + '~') AS [fileType]
			, COALESCE(cl2.cdDesc, '~' + base.fileStatus + '~') AS [fileStatus]
			, dbo.RemoveIllegalXMLCharacters(base.fileDesc) AS [fileDesc]
			, dbo.RemoveIllegalXMLCharacters(ISNULL(base.fileNotes, '') + ' ' + ISNULL(base.fileExternalNotes, '')) AS [fileNotes]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
 		FROM config.dbFile base
			LEFT JOIN dbo.GetCodeLookupDescription('FileType', @UI) cl1 ON cl1.cdCode = base.fileType
			LEFT JOIN dbo.GetCodeLookupDescription('FileStatus', @UI) cl2 ON cl2.cdCode = base.fileStatus
			LEFT JOIN search.FormatMSPermForAS(@EntityName, @T1) obj ON obj.ID = base.fileID
			INNER JOIN @TBatch tb ON tb.Id = base.fileID
		FOR XML RAW
		)
	
	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			fileID
			, 'I'
		FROM config.dbFile
		WHERE fileID > @fileID
		ORDER BY fileID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @fileID
		ORDER BY Id

	SET @fileID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
