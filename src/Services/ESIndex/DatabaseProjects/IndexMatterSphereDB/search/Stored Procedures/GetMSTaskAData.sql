CREATE PROCEDURE search.GetMSTaskAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'TASK'
	, @X XML
	, @tskID BIGINT
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

DECLARE @fileDescColumnId INT = COLUMNPROPERTY(OBJECT_ID('config.dbFile'),'fileDesc', 'ColumnId');

DECLARE @TDeleted TABLE (OldID BIGINT, TaskID BIGINT, DeletedGuid UNIQUEIDENTIFIER)

INSERT INTO @TDeleted 
	SELECT s.OldFileID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbMatterMergeDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbTasks/tskID') as m(c)

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
			tskID
			, 'I'
		FROM dbo.dbTasks
		ORDER BY tskID
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			tskID
			, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
		FROM (
			SELECT tskID
				, CT.SYS_CHANGE_OPERATION AS Op
			FROM CHANGETABLE( CHANGES dbo.dbTasks, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
			UNION
			SELECT dbTasks.tskID 
				, 'U'
			FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
				INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
				INNER JOIN dbo.dbTasks ON (dbTasks.tskType = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'TASKTYPE')
			UNION 
			SELECT dbTasks.tskID
				, 'U'
			FROM CHANGETABLE(CHANGES relationship.UserGroup_file, @COPIEDVERSION) AS CT
				LEFT JOIN relationship.UserGroup_file ON CT.RelationshipID = UserGroup_file.RelationshipID
				LEFT JOIN relationship.Usergroup_File_Delete ON CT.RelationshipID = Usergroup_File_Delete.ID
				INNER JOIN dbo.dbTasks ON dbTasks.fileID = ISNULL(UserGroup_file.fileID, Usergroup_File_Delete.fileID)
			UNION
			SELECT dbTasks.tskID 
				, 'U'
			FROM dbTasks
			INNER JOIN (SELECT CT.fileID as [fileID]
				FROM CHANGETABLE(CHANGES config.dbFile, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION = 'U'
					AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@fileDescColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
			ON dbTasks.fileID = CTT.fileID
			) r
		GROUP BY r.tskID

		INSERT INTO @TInc(Id, Op)
		SELECT 
			tskID
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE( CHANGES dbo.dbTasks, @COPIEDVERSION) AS CT
			WHERE CT.SYS_CHANGE_OPERATION = 'D'

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op
	END

	SET @tskID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

	WHILE @tskID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT d.DeletedGuid AS [id]
				, d.TaskID as [mattersphereid]
				, 'true' AS [entityDeleted]
			FROM @TBatch tb
			INNER JOIN @TDeleted d ON d.TaskID = tb.Id
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
			INNER JOIN dbo.dbTasks base ON base.tskID = tb.Id

		SET @X = (
			SELECT base.rowguid AS [id]
				, base.fileID AS [file-id]
				, dbo.RemoveIllegalXMLCharacters([file].fileDesc) as [fileDesc]
				, base.docID as [documentId]
				, COALESCE(cl1.cdDesc, '~' + base.tskType + '~') AS [taskType]
				, dbo.RemoveIllegalXMLCharacters(base.tskDesc) AS [tskDesc]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, dbo.RemoveIllegalXMLCharacters(base.tskDesc) AS [title]
				, base.tskID AS [mattersphereid]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM dbo.dbTasks base
				LEFT JOIN config.dbFile [file] ON [file].fileID = base.fileID
				LEFT JOIN dbo.GetCodeLookupDescription('TASKTYPE', @UI) cl1 ON cl1.cdCode = base.tskType
				LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.tskID)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.fileID
		FROM @TBatch rb
			INNER JOIN dbo.dbTasks p ON p.tskID = rb.Id
		WHERE rb.Op = 'I'
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSFileAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

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
				tskID
				, 'I'
			FROM dbo.dbTasks
			WHERE tskID > @tskID
			ORDER BY tskID
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP(@BatchSize)
				Id
				, Op
			FROM @TInc
			WHERE Id > @tskID
			ORDER BY Id, Op
	
		SET @tskID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	END
END
ELSE 
BEGIN 
	SET @X = (

		SELECT base.rowguid AS [id]
			, base.fileID AS [file-id]
			, dbo.RemoveIllegalXMLCharacters([file].fileDesc) as [fileDesc]
			, base.docID as [documentId]
			, COALESCE(cl1.cdDesc, '~' + base.tskType + '~') AS [taskType]
			, dbo.RemoveIllegalXMLCharacters(base.tskDesc) AS [tskDesc]
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.tskID FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM dbo.dbTasks base
			LEFT JOIN config.dbFile [file] ON [file].fileID = base.fileID
			LEFT JOIN dbo.GetCodeLookupDescription('TASKTYPE', @UI) cl1 ON cl1.cdCode = base.tskType
			LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
		/**** CUSTOM EXTDATA BEGIN *****/
		/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.tskID)
		FOR XML RAW
		)
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
