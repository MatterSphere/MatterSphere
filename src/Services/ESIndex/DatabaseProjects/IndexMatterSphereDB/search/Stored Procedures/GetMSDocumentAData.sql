CREATE PROCEDURE search.GetMSDocumentAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE  @OBJTYPE NVARCHAR(100) = 'DOCUMENT'
	, @X XML
	, @docID BIGINT
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
	, @ReindexFailedItems BIT
	, @ProcessOrder BIT

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

DECLARE @TDeleted TABLE (OldID BIGINT, DocID BIGINT, DeletedGuid UNIQUEIDENTIFIER)

INSERT INTO @TDeleted 
	SELECT s.OldFileID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbMatterMergeDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbDocument/docID') as m(c)

DECLARE @EmailEnabled BIT = ISNULL((SELECT IndexingEnabled FROM search.ESIndexTable WHERE ObjectType = 'EMAIL'), 0)
DECLARE @DocumentDateLimit DATETIME = (SELECT DocumentDateLimit FROM search.ChangeVersionControl)
DECLARE @PreviousDocumentDateLimit DATETIME = (SELECT PreviousDocumentDateLimit FROM search.ChangeVersionControl)

DECLARE @clientNameColumnId INT = COLUMNPROPERTY(OBJECT_ID('config.dbClient'),'clName', 'ColumnId');
DECLARE @fileDescColumnId INT = COLUMNPROPERTY(OBJECT_ID('config.dbFile'),'fileDesc', 'ColumnId');

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
		, @ReindexFailedItems = CASE ReindexFailedItems WHEN 2 THEN 1 ELSE 0 END
		, @ProcessOrder = ProcessOrder
	FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
	WHERE ObjectType = @OBJTYPE

	IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
		SET @BULKREQUIRED = 1

	IF @BULKREQUIRED = 1
		IF @ProcessOrder = 0
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP (@BatchSize) 
				d.docID
				, 'I'
			FROM config.dbDocument d 
			WHERE (@EmailEnabled = 0 OR d.docType <> 'EMAIL') 
				AND (d.Updated >= COALESCE(@DocumentDateLimit, Updated) AND d.Updated <= COALESCE(@PreviousDocumentDateLimit, Updated))
			ORDER BY d.docID
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP (@BatchSize) 
				d.docID
				, 'I'
			FROM config.dbDocument d 
			WHERE (@EmailEnabled = 0 OR d.docType <> 'EMAIL')
				AND (d.Updated >= COALESCE(@DocumentDateLimit, Updated) AND d.Updated <= COALESCE(@PreviousDocumentDateLimit, Updated))
			ORDER BY d.docID DESC
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
			WHERE CT.SYS_CHANGE_OPERATION <> 'D'
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
		SELECT D.docID
			, 'U'
		FROM config.dbDocument AS D
			INNER JOIN search.ExtensionToReindex AS E ON E.Extension = D.docExtension AND E.Flag = 1
		UNION
		SELECT D.docID
			, 'U'
		FROM config.dbDocument AS D
			INNER JOIN search.ESIndexDocumentLog AS R ON R.docID = D.docId AND R.ErrB = 1 AND @ReindexFailedItems = 1
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
		UNION
		SELECT D.docID
			, 'U'
		FROM config.dbDocument AS D
		INNER JOIN (SELECT CT.clID as [clID]
			FROM CHANGETABLE(CHANGES config.dbClient, @COPIEDVERSION) AS CT
			WHERE CT.SYS_CHANGE_OPERATION = 'U'
				AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@clientNameColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
		ON D.clID = CTT.clID
		UNION
		SELECT D.docID
			, 'U'
		FROM config.dbDocument AS D
		INNER JOIN (SELECT CT.fileID as [fileID]
			FROM CHANGETABLE(CHANGES config.dbFile, @COPIEDVERSION) AS CT
			WHERE CT.SYS_CHANGE_OPERATION = 'U'
				AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@fileDescColumnId, CT.SYS_CHANGE_COLUMNS) = 1) CTT
		ON D.fileID = CTT.fileID
		) r
			INNER JOIN config.dbDocument AS d ON d.docId = r.docID
		WHERE (@EmailEnabled = 0 OR d.docType <> 'EMAIL') AND d.Updated >= COALESCE(@DocumentDateLimit, Updated)
		GROUP BY r.docId

		INSERT INTO @TInc(Id, Op)
		SELECT 
			docID
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE( CHANGES config.dbDocument, @COPIEDVERSION) AS CT
			WHERE CT.SYS_CHANGE_OPERATION = 'D'

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op

		SET @ProcessOrder = 0
	END

	IF @ProcessOrder = 0
		SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	ELSE 
		SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id)

	WHILE @docID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT d.DeletedGuid AS [id]
				, d.DocID as [mattersphereid]
				, 'true' AS [entityDeleted] 
			FROM @TBatch tb
			INNER JOIN @TDeleted d ON d.DocID = tb.Id
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
				, base.AssocID AS [associate-id]
				, auth.usrFullName AS [authorType]
				, base.clID as [client-id]
				, client.clNo as [clientNum]
				, client.clName as [clientName]
				, '' AS [docContents]
				, CASE base.docDeleted WHEN 1 THEN 'true' ELSE 'false' END AS [docDeleted]
				, dbo.RemoveIllegalXMLCharacters(base.docDesc) AS [docDesc]
				, LOWER(REPLACE(base.docExtension,'.','')) AS [documentExtension]
				, COALESCE(cl1.cdDesc, '~' + base.docType + '~') AS [documentType]
				, base.fileID as [file-id]
				, dbo.RemoveIllegalXMLCharacters([file].fileDesc) as [fileDesc]
				, dbo.RemoveIllegalXMLCharacters(base.docDesc) AS [title]
				, base.docID AS [mattersphereid]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, CASE WHEN EXISTS (SELECT 1 FROM CHANGETABLE(CHANGES config.dbClient, @COPIEDVERSION) AS CT
						WHERE CT.SYS_CHANGE_OPERATION = 'U'
							AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@clientNameColumnId, CT.SYS_CHANGE_COLUMNS) = 1
							AND CT.clID = base.clID)
						THEN 'true' 
					WHEN EXISTS (SELECT 1 FROM CHANGETABLE(CHANGES config.dbFile, @COPIEDVERSION) AS CT
						WHERE CT.SYS_CHANGE_OPERATION = 'U'
							AND CHANGE_TRACKING_IS_COLUMN_IN_MASK(@fileDescColumnId, CT.SYS_CHANGE_COLUMNS) = 1
							AND CT.fileID = base.fileID)
						THEN 'true' 
					ELSE 'false'
					END AS [skipContent]
				, CONCAT(dir.dirPath + '\', base.[docFileName]) AS [Sys_FileName]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM config.dbDocument base 
				LEFT JOIN config.dbClient client ON base.clID = client.clID  
				LEFT JOIN config.dbFile [file]   ON base.fileID = [file].fileID
				LEFT JOIN dbo.dbDirectory dir ON dir.dirID = base.docdirID
				LEFT OUTER JOIN dbo.dbUser auth ON base.docAuthoredBy = auth.usrid
				LEFT JOIN dbo.GetCodeLookupDescription('DOCTYPE', @UI) cl1 ON cl1.cdCode = base.docType
				LEFT JOIN search.FormatMSPermForAS(@OBJTYPE, @T1) obj ON obj.ID = base.docID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.docID)
			FOR XML RAW
			)
	
		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.clID
		FROM @TBatch rb
			INNER JOIN config.dbDocument p ON p.docID = rb.Id
		WHERE rb.Op = 'I'
			AND p.docDeleted <> 1
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD
			
			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.fileID
			FROM @TBatch rb
				INNER JOIN config.dbDocument p ON p.docID = rb.Id
			WHERE rb.Op = 'I'
				AND p.docDeleted <> 1
			
			EXEC search.GetMSFileAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD
			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.AssocID
			FROM @TBatch rb
				INNER JOIN config.dbDocument p ON p.docID = rb.Id
			WHERE rb.Op = 'I'
				AND p.docDeleted <> 1
			
			EXEC search.GetMSAssociateAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

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
			IF @ProcessOrder = 0
				INSERT INTO @TBatch
				SELECT TOP (@BatchSize)
					d.docID
					, 'I'
				FROM config.dbDocument d 
				WHERE (@EmailEnabled = 0 OR d.docType <> 'EMAIL')
					AND d.docID > @docId
					AND (d.Updated >= COALESCE(@DocumentDateLimit, Updated) AND d.Updated <= COALESCE(@PreviousDocumentDateLimit, Updated))
				ORDER BY d.docID
			ELSE 
				INSERT INTO @TBatch
				SELECT TOP (@BatchSize)
					d.docID
					, 'I'
				FROM config.dbDocument d 
				WHERE (@EmailEnabled = 0 OR d.docType <> 'EMAIL')
					AND d.docID < @docId
					AND (d.Updated >= COALESCE(@DocumentDateLimit, Updated) AND d.Updated <= COALESCE(@PreviousDocumentDateLimit, Updated))
				ORDER BY d.docID DESC
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP(@BatchSize)
				Id
				, Op
			FROM @TInc
			WHERE Id > @docId
			ORDER BY Id, Op

		IF @ProcessOrder = 0
			SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
		ELSE
			SET @docID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id)
	END
END
ELSE
BEGIN 
	SET @X = (
			SELECT base.rowguid AS [id]
				, base.AssocID AS [associate-id]
				, auth.usrFullName AS [authorType]
				, base.clID as [client-id]
				, client.clNo as [clientNum]
				, client.clName as [clientName]
				, '' AS [docContents]
				, CASE base.docDeleted WHEN 1 THEN 'true' ELSE 'false' END  AS [docDeleted]
				, dbo.RemoveIllegalXMLCharacters(base.docDesc) AS [docDesc]
				, LOWER(REPLACE(base.docExtension,'.','')) AS [documentExtension]
				, COALESCE(cl1.cdDesc, '~' + base.docType + '~') AS [documentType]
				, base.fileID as [file-id]
				, dbo.RemoveIllegalXMLCharacters([file].fileDesc) as [fileDesc]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
				, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.docId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
			FROM config.dbDocument base 
				LEFT JOIN config.dbClient client ON base.clID = client.clID  
				LEFT JOIN config.dbFile [file]   ON base.fileID = [file].fileID
				LEFT OUTER JOIN dbo.dbUser auth ON base.docAuthoredBy = auth.usrid
				LEFT JOIN dbo.GetCodeLookupDescription('DOCTYPE', @UI) cl1 ON cl1.cdCode = base.docType
				/**** CUSTOM EXTDATA BEGIN *****/
				/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.docId)
			FOR XML RAW)

	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
