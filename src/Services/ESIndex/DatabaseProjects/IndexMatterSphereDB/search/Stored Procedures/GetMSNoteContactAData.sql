CREATE PROCEDURE search.GetMSNoteContactAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @BULKREQUIRED BIT, @COPIEDVERSION BIGINT)
AS
SET NOCOUNT ON;
DECLARE @OBJTYPE NVARCHAR(100) = 'NOTE'
	, @X XML
	, @contID BIGINT 

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize) 
		n.contID
		, 'U'
	FROM config.dbContact n
	WHERE n.contNotes IS NOT NULL
	ORDER BY n.contID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT CT.contID
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbContact, @COPIEDVERSION) AS CT
		INNER JOIN config.dbContact n ON n.contID = CT.contID
	WHERE n.contNotes IS NOT NULL
	UNION
	SELECT
		n.contID 
		, 'U'
	FROM CHANGETABLE( CHANGES relationship.UserGroup_Contact, @COPIEDVERSION) AS CT
		LEFT OUTER JOIN relationship.UserGroup_Contact ON CT.RelationshipID = UserGroup_Contact.RelationshipID
		LEFT OUTER JOIN relationship.Usergroup_Contact_Delete ON CT.RelationshipID = Usergroup_Contact_Delete.ID
		INNER JOIN config.dbContact n ON n.contID = ISNULL(UserGroup_Contact.contactID, Usergroup_Contact_Delete.contactID) 
	WHERE n.contNotes IS NOT NULL

	INSERT INTO @TInc(Id, Op)
	SELECT 
		contID 
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbContact, @COPIEDVERSION) AS CT
		WHERE CT.SYS_CHANGE_OPERATION = 'D'

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id, Op
END

SET @contID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

DECLARE @NoteSource NVARCHAR(100) = 'Contact'

WHILE @contID IS NOT NULL
BEGIN
	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), tb.Id, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000002') AS UNIQUEIDENTIFIER) AS [id]
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
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), n.contID, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000002') AS UNIQUEIDENTIFIER) AS [id]
			, @NoteSource AS [noteSource]
			, n.contID AS [contactId]
			, dbo.RemoveIllegalXMLCharacters(n.contNotes) AS [note]
			, CONCAT('Note from ', @NoteSource) AS [title]
			, n.contID AS [mattersphereid]
			, CAST(NULL AS DATETIME) AS [modifieddate] 
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
		FROM config.dbContact n 
			LEFT JOIN search.FormatMSPermForAS('CONTACT', @T1) obj ON obj.ID = n.contId
		WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = n.contID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;
	
	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize) 
			n.contID
			, 'U'
		FROM config.dbContact n
		WHERE n.contNotes IS NOT NULL
			AND n.contID > @contID
		ORDER BY n.contID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @contID
		ORDER BY Id, Op

	SET @contID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END

