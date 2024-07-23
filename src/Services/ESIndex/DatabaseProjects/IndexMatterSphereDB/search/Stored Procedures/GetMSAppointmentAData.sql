CREATE PROCEDURE search.GetMSAppointmentAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'APPOINTMENT'
	, @X XML
	, @appID BIGINT
	, @RelFlag BIT

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @RelD AS search.ESRelationship
DECLARE @RelC AS search.ESRelationship
DECLARE @T1 AS search.ESUGPD

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT
	, @TABLENAME NVARCHAR(128)

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

DECLARE @clientNameColumnId INT = COLUMNPROPERTY(OBJECT_ID('config.dbClient'),'clName', 'ColumnId');
DECLARE @fileDescColumnId INT = COLUMNPROPERTY(OBJECT_ID('config.dbFile'),'fileDesc', 'ColumnId');

DECLARE @TDeleted TABLE (OldID BIGINT, AppID BIGINT, DeletedGuid UNIQUEIDENTIFIER)

INSERT INTO @TDeleted 
	SELECT s.OldFileID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbMatterMergeDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbAppointments/appID') as m(c)

INSERT INTO @TDeleted 
	SELECT s.ClID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbClientDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbAppointments/appID') as m(c)

IF @RelType IS NULL
	SET @RelFlag = 0
ELSE 
	SET @RelFlag = 1

IF @RelFlag = 0
BEGIN 
	SELECT 
		@BULKREQUIRED = CASE WHEN changeVersionControl.FullCopyRequired = 1 OR ESIndexTable.FullCopyRequired = 1 THEN 1 ELSE 0 END
		, @COPIEDVERSION = LastCopiedVersion
		, @WORKINGVERSION = WorkingVersion
		, @TABLENAME = tablename
	FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
	WHERE ObjectType = @OBJTYPE

	IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
		SET @BULKREQUIRED = 1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			appID
			, 'I'
		FROM dbo.dbAppointments
		ORDER BY appID
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			appID
			, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
		FROM (
			SELECT appID
				, CT.SYS_CHANGE_OPERATION AS Op
			FROM CHANGETABLE( CHANGES dbo.dbAppointments, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
			UNION
			SELECT dbAppointments.appID 
				, 'U'
			FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
				INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
				INNER JOIN dbo.dbAppointments ON (dbAppointments.appType = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'APPTYPE')
			UNION 
			SELECT dbAppointments.appID
				, 'U'
			FROM CHANGETABLE(CHANGES relationship.UserGroup_file, @COPIEDVERSION) AS CT
				LEFT JOIN relationship.UserGroup_file ON CT.RelationshipID = UserGroup_file.RelationshipID
				LEFT JOIN relationship.Usergroup_File_Delete ON CT.RelationshipID = Usergroup_File_Delete.ID
				INNER JOIN dbAppointments ON dbAppointments.fileID = ISNULL(UserGroup_file.fileID, Usergroup_File_Delete.fileID)
			UNION
			SELECT dbAppointments.appID
				, 'U'
			FROM dbAppointments
			INNER JOIN (SELECT CT.clID as [clID]
				FROM CHANGETABLE(CHANGES config.dbClient, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION = 'U'
					AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@clientNameColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
			ON dbAppointments.clID = CTT.clID
			UNION
			SELECT dbAppointments.appID
				, 'U'
			FROM dbAppointments
			INNER JOIN (SELECT CT.fileID as [fileID]
				FROM CHANGETABLE(CHANGES config.dbFile, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION = 'U'
					AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@fileDescColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
			ON dbAppointments.fileID = CTT.fileID
			) r
		GROUP BY appID

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

	WHILE @appID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT d.DeletedGuid AS [id]
				, d.AppID as [mattersphereid]
				, 'true' AS [entityDeleted]
			FROM @TBatch tb
			INNER JOIN @TDeleted d ON d.AppID = tb.Id
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
			SELECT base.rowguid AS [id]
				, base.fileID AS [file-id]
				, [file].fileDesc AS [fileDesc]
				, base.clID AS [client-id]
				, client.clNo as [clientNum]
				, client.clName as [clientName]
				, base.AssocID AS [associateId]
				, base.docID as [documentId]
				, COALESCE(cl1.cdDesc, '~' + base.appType + '~') AS [appointmentType]
				, dbo.RemoveIllegalXMLCharacters(base.appDesc) AS [appDesc]
				, base.appLocation AS [appLocation]
				, base.appDate
				, base.appEndDate
				, base.appTimeZone
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, dbo.RemoveIllegalXMLCharacters(base.appDesc) AS [title]
				, base.appId AS [mattersphereid]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM dbo.dbAppointments base
				LEFT JOIN config.dbClient client ON base.clID = client.clID
				LEFT JOIN config.dbFile [file] ON base.fileID = [file].fileID
				LEFT JOIN dbo.GetCodeLookupDescription('APPTYPE', @UI) cl1 ON cl1.cdCode = base.appType
				LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.appId)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.fileID
		FROM @TBatch rb
			INNER JOIN dbo.dbAppointments p ON p.appId = rb.Id
		WHERE rb.Op = 'I'
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSFileAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD

			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.clID
			FROM @TBatch rb
				INNER JOIN dbo.dbAppointments p ON p.appId = rb.Id
			WHERE rb.Op = 'I'

			EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD
			/**** CUSTOM PARENT END *****/
		END

		IF @BULKREQUIRED = 0 
		BEGIN
			IF EXISTS(SELECT 1 FROM @TBatch WHERE Op = 'U')
			BEGIN
				no_op:
				/**** CUSTOM CHILD BEGIN *****/
				/**** CUSTOM CHILD END *****/
			END
		END	

		DELETE @TBatch
		DELETE @T1

		IF @BULKREQUIRED = 1
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP (@BatchSize)
				appID
				, 'I'
			FROM dbo.dbAppointments
			WHERE appID > @appID
			ORDER BY appID
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
END
ELSE 
BEGIN 
	SET @X = (

		SELECT base.rowguid AS [id]
			, base.fileID AS [file-id]
			, [file].fileDesc AS [fileDesc]
			, base.clID AS [client-id]
			, client.clNo as [clientNum]
			, client.clName as [clientName]
			, base.AssocID AS [associateId]
			, base.docID as [documentId]
			, COALESCE(cl1.cdDesc, '~' + base.appType + '~') AS [appointmentType]
			, dbo.RemoveIllegalXMLCharacters(base.appDesc) AS [appDesc]
			, base.appLocation AS [appLocation]
			, base.appDate
			, base.appEndDate
			, base.appTimeZone
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.appId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM dbo.dbAppointments base
			LEFT JOIN config.dbClient client ON base.clID = client.clID
			LEFT JOIN config.dbFile [file] ON base.fileID = [file].fileID
			LEFT JOIN dbo.GetCodeLookupDescription('APPTYPE', @UI) cl1 ON cl1.cdCode = base.appType
			LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
		/**** CUSTOM EXTDATA BEGIN *****/
		/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.appID)
		FOR XML RAW
		)
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
