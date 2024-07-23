CREATE PROCEDURE search.GetMSNoteAssociateAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @BULKREQUIRED BIT, @COPIEDVERSION BIGINT)
AS
SET NOCOUNT ON;
DECLARE @OBJTYPE NVARCHAR(100) = 'NOTE'
	, @X XML
	, @assocID BIGINT 	

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize) 
		n.assocID
		, 'U'
	FROM config.dbAssociates n
	WHERE n.assocNotes IS NOT NULL
	ORDER BY n.assocID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT CT.assocID
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbAssociates, @COPIEDVERSION) AS CT
		INNER JOIN config.dbAssociates n ON n.assocID = CT.assocID
	WHERE n.assocNotes IS NOT NULL
	UNION
	SELECT n.assocID 
		, 'U'
	FROM CHANGETABLE( CHANGES relationship.UserGroup_Contact, @COPIEDVERSION) AS [CT]
		LEFT OUTER JOIN relationship.UserGroup_Contact ON CT.RelationshipID = UserGroup_Contact.RelationshipID
		LEFT OUTER JOIN relationship.Usergroup_Contact_Delete ON CT.RelationshipID = Usergroup_Contact_Delete.ID
		INNER JOIN config.dbAssociates n ON n.contID = ISNULL(UserGroup_Contact.ContactID, Usergroup_Contact_Delete.ContactID)
	WHERE n.assocNotes IS NOT NULL

	INSERT INTO @TInc(Id, Op)
	SELECT 
		assocID 
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbAssociates, @COPIEDVERSION) AS CT
		WHERE CT.SYS_CHANGE_OPERATION = 'D'

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id, Op
END

SET @assocID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

DECLARE @NoteSource NVARCHAR(100) = 'Associate'

WHILE @assocID IS NOT NULL
BEGIN
	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), tb.Id, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000004') AS UNIQUEIDENTIFIER) AS [id]
			, tb.Id as [mattersphereid]
			, 'true' AS [entityDeleted] 
		FROM @TBatch tb
		WHERE tb.Op = 'D'
		FOR XML RAW
		)
	IF @X IS NOT NULL
	BEGIN
		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;
	END

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), n.assocID, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000004') AS UNIQUEIDENTIFIER) AS [id]
			, @NoteSource AS [noteSource]
			, n.assocID AS [associateId]
			, n.contID AS [contactId]
			, n.fileID AS [fileId]
			, dbo.RemoveIllegalXMLCharacters(n.assocNotes) AS [note]
			, CONCAT('Note from ', @NoteSource) AS [title]
			, n.assocID AS [mattersphereid]
			, CAST(NULL AS DATETIME) AS [modifieddate] 
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
		FROM config.dbAssociates n 
			LEFT JOIN search.FormatMSPermForAS('ASSOCIATE', @T1) obj ON obj.ID = n.assocId
		WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = n.assocID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;
	
	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize) 
			n.assocID
			, 'U'
		FROM config.dbAssociates n
		WHERE n.assocNotes IS NOT NULL
			AND n.assocID > @assocID
		ORDER BY n.assocID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @assocID
		ORDER BY Id, Op

	SET @assocID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END

