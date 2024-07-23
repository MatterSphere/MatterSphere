CREATE PROCEDURE search.GetMSContactAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'CONTACT'
	, @X XML
	, @contID BIGINT
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
			contID
			, 'I'
		FROM config.dbContact
		ORDER BY contId
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			contID
			, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
		FROM (
			SELECT 
				contID 
				, CT.SYS_CHANGE_OPERATION AS Op
			FROM CHANGETABLE(CHANGES config.dbContact, @COPIEDVERSION) AS [CT]
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
			UNION
			SELECT
				ISNULL(UserGroup_Contact.contactID, Usergroup_Contact_Delete.contactID) 
				, 'U'
			FROM CHANGETABLE( CHANGES relationship.UserGroup_Contact, @COPIEDVERSION) AS CT
				LEFT OUTER JOIN relationship.UserGroup_Contact ON CT.RelationshipID = UserGroup_Contact.RelationshipID
				LEFT OUTER JOIN relationship.Usergroup_Contact_Delete ON CT.RelationshipID = Usergroup_Contact_Delete.ID
			UNION
			SELECT 
				dbContact.contID
				, 'U'
			FROM CHANGETABLE( CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
				INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
				INNER JOIN config.dbContact ON (dbContact.contTypeCode = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'CONTTYPE')
			) r
		GROUP BY contID

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

	WHILE @contID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT cmd.DeletedGuid AS [id]
				, cmd.OldContID as [mattersphereid]
				, 'true' AS [entityDeleted] 
			FROM @TBatch tb
			INNER JOIN dbo.dbContactMergeDelete cmd ON cmd.OldContID = tb.Id
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
				, COALESCE(cl1.cdDesc, '~' + base.contTypeCode + '~') AS [contactType]
				, base.contName AS [contName]
				, base.contDefaultAddress as [address-id]
				, dbo.GetAddress(base.contDefaultAddress, ', ', @UI) AS [address]
				, base.contName AS [title]
				, base.contID AS [mattersphereid]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM config.dbContact base
				LEFT OUTER JOIN dbo.dbAddress a on a.addID = base.contDefaultAddress
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('CONTTYPE', @UI) cl1 ON cl1.cdCode = base.contTypeCode
				LEFT JOIN search.FormatMSPermForAS(@OBJTYPE, @T1) obj ON obj.ID = base.contId
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.contID)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.contDefaultAddress
		FROM @TBatch rb
			INNER JOIN config.dbContact p ON p.contID = rb.Id
		WHERE rb.Op = 'I'

		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSAddressAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD
			/**** CUSTOM PARENT END *****/
		END

		IF @BULKREQUIRED = 0 
		BEGIN
			IF EXISTS(SELECT 1 FROM @TBatch WHERE Op = 'U')
			BEGIN
				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN config.dbAssociates c ON c.contId = rb.Id
				WHERE rb.Op = 'U'

				IF @@ROWCOUNT > 0
					EXEC search.GetMSContactAData @UI = @UI, @Rel = @RelC, @RelType = 'ASSOCIATE'

				DELETE @RelC

				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN dbo.dbClientContacts c ON c.contId = rb.Id
				WHERE rb.Op = 'U'

				IF @@ROWCOUNT > 0
					EXEC search.GetMSContactAData @UI = @UI, @Rel = @RelC, @RelType = 'CCLINK'

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
			contID
			, 'I'
		FROM config.dbContact
		WHERE contID > @contID
		ORDER BY contId
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
END
ELSE 
BEGIN 
	SET @X = (
			SELECT base.rowguid AS [id]
				, COALESCE(cl1.cdDesc, '~' + base.contTypeCode + '~') AS [contactType]
				, base.contName AS [contName]
				, base.contDefaultAddress as [address-id]
				, dbo.GetAddress(base.contDefaultAddress, ', ', @UI) AS [address]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
				, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.contId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
			FROM config.dbContact base
				LEFT OUTER JOIN dbo.dbAddress a on a.addID = base.contDefaultAddress
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('CONTTYPE', @UI) cl1 ON cl1.cdCode = base.contTypeCode
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.contId)
			FOR XML RAW)

	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
