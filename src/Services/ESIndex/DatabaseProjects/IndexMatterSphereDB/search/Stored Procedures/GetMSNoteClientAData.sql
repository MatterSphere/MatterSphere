CREATE PROCEDURE search.GetMSNoteClientAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @BULKREQUIRED BIT, @COPIEDVERSION BIGINT)
AS
SET NOCOUNT ON;
DECLARE @OBJTYPE NVARCHAR(100) = 'NOTE'
	, @X XML
	, @clID BIGINT 

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize) 
		n.clID
		, 'U'
	FROM config.dbClient n
	WHERE n.clNotes IS NOT NULL
	ORDER BY n.clID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT CT.clID
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbClient, @COPIEDVERSION) AS CT
		INNER JOIN config.dbClient n ON n.clID = CT.clID
	WHERE n.clNotes IS NOT NULL
	UNION
	SELECT 
		n.clID
		, 'U'
	FROM CHANGETABLE(CHANGES relationship.UserGroup_Client, @COPIEDVERSION) AS CT
		LEFT OUTER JOIN relationship.UserGroup_Client ON CT.RelationshipID = UserGroup_Client.RelationshipID
		LEFT OUTER JOIN relationship.Usergroup_Client_Delete ON CT.RelationshipID = Usergroup_Client_Delete.ID
		INNER JOIN config.dbClient n ON n.clID = ISNULL(UserGroup_Client.clientID, Usergroup_Client_Delete.clid) 
	WHERE n.clNotes IS NOT NULL

	INSERT INTO @TInc(Id, Op)
	SELECT 
		clID 
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES config.dbClient, @COPIEDVERSION) AS CT
		WHERE CT.SYS_CHANGE_OPERATION = 'D'

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id, Op
END

SET @clID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

DECLARE @NoteSource NVARCHAR(100) = 'Client'

WHILE @clID IS NOT NULL
BEGIN
	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), tb.Id, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000001') AS UNIQUEIDENTIFIER) AS [id]
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
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), n.clID, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000001') AS UNIQUEIDENTIFIER) AS [id]
			, @NoteSource AS [noteSource]
			, n.clID AS [clientId]
			, dbo.RemoveIllegalXMLCharacters(n.clNotes) AS [note]
			, CONCAT('Note from ', @NoteSource) AS [title]
			, n.clID AS [mattersphereid]
			, CAST(NULL AS DATETIME) AS [modifieddate] 
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
		FROM config.dbClient n
			LEFT JOIN search.FormatMSPermForAS('CLIENT', @T1) obj ON obj.ID = n.clID
		WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = n.clID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;
	
	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize) 
			n.clID
			, 'U'
		FROM config.dbClient n
		WHERE n.clNotes IS NOT NULL
			AND n.clID > @clID
		ORDER BY n.clID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @clID
		ORDER BY Id, Op

	SET @clID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
