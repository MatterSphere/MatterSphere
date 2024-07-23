CREATE PROCEDURE search.GetMSNoteAppointmentAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @BULKREQUIRED BIT, @COPIEDVERSION BIGINT)
AS
SET NOCOUNT ON;
DECLARE @OBJTYPE NVARCHAR(100) = 'NOTE'
	, @X XML
	, @appID BIGINT 

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize) 
		n.appID
		, 'U'
	FROM dbo.dbAppointments n
	WHERE n.appNotes IS NOT NULL
	ORDER BY n.appID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT CT.appID
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES dbo.dbAppointments, @COPIEDVERSION) AS CT
		INNER JOIN dbo.dbAppointments n ON n.appID = CT.appID
	WHERE n.appNotes IS NOT NULL
	UNION 
	SELECT n.appID
		, 'U'
	FROM CHANGETABLE(CHANGES relationship.UserGroup_file, @COPIEDVERSION) AS CT
		LEFT JOIN relationship.UserGroup_file ON CT.RelationshipID = UserGroup_file.RelationshipID
		LEFT JOIN relationship.Usergroup_File_Delete ON CT.RelationshipID = Usergroup_File_Delete.ID
		INNER JOIN dbAppointments n ON n.fileID = ISNULL(UserGroup_file.fileID, Usergroup_File_Delete.fileID)
	WHERE n.appNotes IS NOT NULL

	INSERT INTO @TInc(Id, Op)
	SELECT 
		appID 
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE( CHANGES dbo.dbAppointments, @COPIEDVERSION) AS CT
		WHERE CT.SYS_CHANGE_OPERATION = 'D'

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id, Op
END

SET @appID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

DECLARE @NoteSource NVARCHAR(100) = 'Appointment'

WHILE @appID IS NOT NULL
BEGIN
	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), tb.Id, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000005') AS UNIQUEIDENTIFIER) AS [id]
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
	SELECT DISTINCT base.fileID 
	FROM @TBatch tb
		INNER JOIN dbo.dbAppointments base ON base.appID = tb.Id

	SET @X = (
		SELECT CAST(REPLACE(CAST(CONVERT(UNIQUEIDENTIFIER, CONVERT(VARBINARY(8), n.appID, 1)) AS NVARCHAR(MAX)), '-000000000000', '-000000000005') AS UNIQUEIDENTIFIER) AS [id]
			, @NoteSource AS [noteSource]
			, n.appID AS [appoinmentId]
			, n.clID AS [clientId]
			, n.fileID AS [fileId]
			, n.docID  AS [documentId]
			, n.assocID AS [associateId]
			, dbo.RemoveIllegalXMLCharacters(n.appNotes) AS [note]
			, CONCAT('Note from ', @NoteSource) AS [title]
			, n.appID AS [mattersphereid]
			, CAST(NULL AS DATETIME) AS [modifieddate] 
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
		FROM dbo.dbAppointments n 
			LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = n.fileID
		WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = n.appID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;
	
	DELETE @TBatch

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize) 
			n.appID
			, 'U'
		FROM dbo.dbAppointments n
		WHERE n.appNotes IS NOT NULL
			AND n.appID > @appID
		ORDER BY n.appID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @appID
		ORDER BY Id, Op

	SET @appID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END

