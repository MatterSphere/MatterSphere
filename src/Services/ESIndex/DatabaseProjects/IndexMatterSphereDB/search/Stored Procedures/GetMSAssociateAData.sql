CREATE PROCEDURE search.GetMSAssociateAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'ASSOCIATE'
	, @X XML
	, @assocId BIGINT
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

DECLARE @TDeleted TABLE (OldID BIGINT, AssocID BIGINT, DeletedGuid UNIQUEIDENTIFIER)

INSERT INTO @TDeleted 
	SELECT s.OldFileID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbMatterMergeDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbAssociates/assocID') as m(c)

INSERT INTO @TDeleted 
	SELECT s.OldContID
		, m.c.value('text()[1]', 'bigint')
		, m.c.value('@rowguid', 'uniqueidentifier')
	FROM dbo.dbContactMergeDelete as s
		CROSS APPLY s.ExecutionXML.nodes('/log/dbAssociates/assocID') as m(c)

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
			assocId
			, 'I'
		FROM config.dbAssociates
		ORDER BY assocId
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			assocId
			, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
		FROM (
			SELECT assocID
				, CT.SYS_CHANGE_OPERATION AS Op
			FROM CHANGETABLE( CHANGES config.dbAssociates, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
			UNION
			SELECT dbAssociates.assocID 
				, 'U'
			FROM CHANGETABLE( CHANGES relationship.UserGroup_Contact, @COPIEDVERSION) AS [CT]
				LEFT OUTER JOIN relationship.UserGroup_Contact ON CT.RelationshipID = UserGroup_Contact.RelationshipID
				LEFT OUTER JOIN relationship.Usergroup_Contact_Delete ON CT.RelationshipID = Usergroup_Contact_Delete.ID
				INNER JOIN config.dbAssociates ON dbAssociates.contID = ISNULL(UserGroup_Contact.ContactID, Usergroup_Contact_Delete.ContactID)
			UNION
			SELECT dbAssociates.assocID 
				, 'U'
			FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
				INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
				INNER JOIN config.dbAssociates ON (dbAssociates.assoctype = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'SUBASSOC')
			) r
		GROUP BY assocId

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

	SET @assocId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

	WHILE @assocId IS NOT NULL
	BEGIN
		SET @X = (
			SELECT d.DeletedGuid AS [id]
				, d.AssocID as [mattersphereid]
				, 'true' AS [entityDeleted] 
			FROM @TBatch tb
			INNER JOIN @TDeleted d ON d.AssocID = tb.Id
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
				, base.fileID AS [file-id]
				, base.contID AS [contact-id]
				, COALESCE(cl1.cdDesc, '~' + base.assoctype + '~') AS [associateType]
				, dbo.RemoveIllegalXMLCharacters(base.assocHeading) AS [assocHeading]
				, base.assocSalut AS [assocSalut]
				, base.assocAddressee AS [assocAddressee]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, base.assocSalut AS [title]
				, base.assocId AS [mattersphereid]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM config.dbAssociates base
				LEFT JOIN [dbo].[GetCodeLookupDescription]('SUBASSOC', @UI) cl1 ON cl1.cdCode = base.assoctype
				LEFT JOIN search.FormatMSPermForAS(@OBJTYPE, @T1) obj ON obj.ID = base.assocId
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.assocId)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.fileID
		FROM @TBatch rb
			INNER JOIN config.dbAssociates p ON p.assocId = rb.Id
		WHERE rb.Op = 'I'
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSFileAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD

			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.contID
			FROM @TBatch rb
				INNER JOIN config.dbAssociates p ON p.assocId = rb.Id
			WHERE rb.Op = 'I'

			EXEC search.GetMSContactAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

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
					INNER JOIN config.dbDocument c ON c.AssocID = rb.Id
				WHERE rb.Op = 'U' AND c.Updated >= COALESCE(@DocumentDateLimit, Updated)

				IF @@ROWCOUNT > 0
					EXEC search.GetMSAssociateAData @UI = @UI, @Rel = @RelC, @RelType = 'DOCUMENT'

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
				assocId
				, 'I'
			FROM config.dbAssociates
			WHERE assocId > @assocId
			ORDER BY assocId
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP(@BatchSize)
				Id
				, Op
			FROM @TInc
			WHERE Id > @assocId
			ORDER BY Id, Op
	
		SET @assocId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	END
END
ELSE 
BEGIN 
	SET @X = (
		SELECT base.rowguid AS [id]
			, base.fileID AS [file-id]
			, base.contID AS [contact-id]
			, COALESCE(cl1.cdDesc, '~' + base.assoctype + '~') AS [associateType]
			, dbo.RemoveIllegalXMLCharacters(base.assocHeading) AS [assocHeading]
			, base.assocSalut AS [assocSalut]
			, base.assocAddressee AS [assocAddressee]
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.assocId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM config.dbAssociates base
			LEFT JOIN [dbo].[GetCodeLookupDescription]('SUBASSOC', @UI) cl1 ON cl1.cdCode = base.assoctype
		/**** CUSTOM EXTDATA BEGIN *****/
		/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.assocId)
		FOR XML RAW
		)
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
