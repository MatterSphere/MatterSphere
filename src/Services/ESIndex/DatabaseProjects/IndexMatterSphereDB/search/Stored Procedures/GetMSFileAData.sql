CREATE PROCEDURE search.GetMSFileAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'FILE'
	, @X XML
	, @fileID BIGINT
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
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
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
			UNION
			SELECT dbFile.fileID
				, 'U'
			FROM config.dbFile
			INNER JOIN (SELECT CT.clID as [clID]
				FROM CHANGETABLE(CHANGES config.dbClient, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION = 'U'
					AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@clientNameColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
			ON dbFile.clID = CTT.clID
			) r
		GROUP BY r.fileID

		INSERT INTO @TInc(Id, Op)
		SELECT 
			fileID 
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE( CHANGES config.dbfile, @COPIEDVERSION) AS CT
			WHERE CT.SYS_CHANGE_OPERATION = 'D'

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op
	END

	SET @fileID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

	WHILE @fileID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT mmd.DeletedGuid AS [id]
				, mmd.OldFileID as [mattersphereid]
				, 'true' AS [entityDeleted] 
			FROM @TBatch tb
			INNER JOIN dbo.dbMatterMergeDelete mmd ON mmd.OldFileID = tb.Id
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
			SELECT base.rowguid AS [id]
				, base.clID AS [client-id]
				, client.clNo as [clientNum]
				, client.clName as [clientName]
				, dbo.RemoveIllegalXMLCharacters(base.fileDesc) AS [fileDesc]
				, COALESCE(cl1.cdDesc, '~' + base.fileType + '~') AS [fileType]
				, COALESCE(cl2.cdDesc, '~' + base.fileStatus + '~') AS [fileStatus]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, dbo.RemoveIllegalXMLCharacters(base.fileDesc) AS [title]
				, base.fileID AS [mattersphereid]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
 			FROM config.dbFile base
				LEFT JOIN config.dbClient client ON client.clID = base.clID
				LEFT JOIN dbo.GetCodeLookupDescription('FileType', @UI) cl1 ON cl1.cdCode = base.fileType
				LEFT JOIN dbo.GetCodeLookupDescription('FileStatus', @UI) cl2 ON cl2.cdCode = base.fileStatus
				LEFT JOIN search.FormatMSPermForAS(@OBJTYPE, @T1) obj ON obj.ID = base.fileID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.fileID)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.clID
		FROM @TBatch rb
			INNER JOIN config.dbFile p ON p.fileID = rb.Id
		WHERE rb.Op = 'I'
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD
			/**** CUSTOM PARENT END *****/
		END

		IF @BULKREQUIRED = 0 
		BEGIN
			IF EXISTS(SELECT 1 FROM @TBatch WHERE Op = 'U')
			BEGIN
				DECLARE @DocumentDateLimit DATETIME = (SELECT DocumentDateLimit FROM search.ChangeVersionControl)

				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN config.dbDocument c ON c.fileId = rb.Id
				WHERE rb.Op = 'U' AND c.Updated >= COALESCE(@DocumentDateLimit, Updated)

				IF @@ROWCOUNT > 0
					EXEC search.GetMSFileAData @UI = @UI, @Rel = @RelC, @RelType = 'DOCUMENT'

				DELETE @RelC
				/**** CUSTOM CHILD BEGIN *****/
				/**** CUSTOM CHILD END *****/
			END
		END	

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
			ORDER BY Id, Op

		SET @fileID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	END
END
ELSE 
BEGIN 
	INSERT INTO @T1
	SELECT DISTINCT RelPK 
	FROM @Rel

	SET @X = (
			SELECT base.rowguid AS [id]
				, base.clID AS [client-id]
				, client.clNo as [clientNum]
				, client.clName as [clientName]
				, dbo.RemoveIllegalXMLCharacters(base.fileDesc) AS [fileDesc]
				, COALESCE(cl1.cdDesc, '~' + base.fileType + '~') AS [fileType]
				, COALESCE(cl2.cdDesc, '~' + base.fileStatus + '~') AS [fileStatus]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
				, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.fileId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
 			FROM config.dbFile base
				LEFT JOIN config.dbClient client ON base.clID = client.clID
				LEFT JOIN dbo.GetCodeLookupDescription('FileType', @UI) cl1 ON cl1.cdCode = base.fileType
				LEFT JOIN dbo.GetCodeLookupDescription('FileStatus', @UI) cl2 ON cl2.cdCode = base.fileStatus
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.fileId)
			FOR XML RAW)

	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END