CREATE PROCEDURE search.GetMSClientAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'CLIENT'
	, @X XML	
	, @RelFlag BIT
	, @clID BIGINT

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
		INSERT INTO @TBatch
		SELECT TOP(@BatchSize) 
			clID
			, 'I'
		FROM config.dbClient
		ORDER BY clID;
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			clID
			, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
		FROM (
			SELECT 
				clID
				, CT.SYS_CHANGE_OPERATION AS Op 
			FROM CHANGETABLE( CHANGES config.dbClient, @COPIEDVERSION) AS CT
				WHERE CT.SYS_CHANGE_OPERATION <> 'D'
			UNION
			SELECT 
				ISNULL(UserGroup_Client.clientID, Usergroup_Client_Delete.clid) 
				, 'U'
			FROM CHANGETABLE(CHANGES relationship.UserGroup_Client, @COPIEDVERSION) AS CT
				LEFT OUTER JOIN relationship.UserGroup_Client ON CT.RelationshipID = UserGroup_Client.RelationshipID
				LEFT OUTER JOIN relationship.Usergroup_Client_Delete ON CT.RelationshipID = Usergroup_Client_Delete.ID
			UNION
			SELECT 
				dbClient.clID
				, 'U'
			FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
				INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
				INNER JOIN config.dbClient ON (dbClient.cltypeCode = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'CLTYPE')
			) r
		GROUP BY clId

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

	WHILE @clID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT cd.DeletedGuid AS [id]
				, cd.ClID as [mattersphereid]
				, 'true' AS [entityDeleted] 
			FROM @TBatch tb
			INNER JOIN dbo.dbClientDelete cd ON cd.ClID = tb.Id
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
				, base.clDefaultAddress AS [address-id]
				, COALESCE(cl1.cdDesc, '~' + base.cltypeCode + '~') AS [clientType]
				, CONVERT(NVARCHAR(MAX), base.clNotes) AS [clientNotes]
				, base.clNo AS [clientNum]
				, base.clName AS [name]
				, CONCAT(base.clNo, ': ', base.clName) AS [title]
				, base.clID AS [mattersphereid]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			FROM config.dbClient base
				LEFT JOIN dbo.GetCodeLookupDescription('CLTYPE', @UI) cl1 ON cl1.cdCode = base.cltypeCode
				LEFT JOIN search.FormatMSPermForAS(@OBJTYPE, @T1) obj ON obj.ID = base.clID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.clID)			
			FOR XML RAW)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.clDefaultAddress
		FROM @TBatch rb
			INNER JOIN config.dbClient p ON p.clID = rb.Id
		WHERE rb.Op = 'I'
			AND p.clDefaultAddress IS NOT NULL

		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSAddressAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 
		END
		DELETE @RelD
		/**** CUSTOM PARENT END *****/

		IF @BULKREQUIRED = 0 
		BEGIN
			IF EXISTS(SELECT 1 FROM @TBatch WHERE Op = 'U')
			BEGIN
				DECLARE @DocumentDateLimit DATETIME = (SELECT DocumentDateLimit FROM search.ChangeVersionControl)

				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN config.dbDocument c ON c.clId = rb.Id
				WHERE rb.Op = 'U' AND c.Updated >= COALESCE(@DocumentDateLimit, Updated)

				IF @@ROWCOUNT > 0
					EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelC, @RelType = 'DOCUMENT'

				DELETE @RelC

				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN config.dbFile c ON c.clId = rb.Id
				WHERE rb.Op = 'U'

				IF @@ROWCOUNT > 0
					EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelC, @RelType = 'FILE'

				DELETE @RelC

				INSERT INTO @RelC (Id, RelPK)
				SELECT c.rowguid
					, rb.Id
				FROM @TBatch rb
					INNER JOIN dbo.dbClientContacts c ON c.clId = rb.Id
				WHERE rb.Op = 'U'

				IF @@ROWCOUNT > 0
					EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelC, @RelType = 'CCLINK'

				DELETE @RelC
				/**** CUSTOM CHILD BEGIN *****/
				/**** CUSTOM CHILD END *****/
			END
		END	

		DELETE @TBatch
		DELETE @T1

		IF @BULKREQUIRED = 1
			INSERT INTO @TBatch
			SELECT TOP(@BatchSize) 
				clID
				, 'I'
			FROM config.dbClient
			WHERE clId > @clID
			ORDER BY clID;
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
END
ELSE 
BEGIN 
	SET @X = (
		SELECT base.rowguid AS [id]
			, base.clDefaultAddress AS [address-id]
			, COALESCE(cl1.cdDesc, '~' + base.cltypeCode + '~') AS [clientType]
			, CONVERT(NVARCHAR(MAX), base.clNotes) AS [clientNotes]
			, base.clNo AS [clientNum]
			, base.clName AS [name]
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.clId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		/**** CUSTOM COLUMNS BEGIN *****/
		/**** CUSTOM COLUMNS END *****/
		FROM config.dbClient base 
			LEFT JOIN dbo.GetCodeLookupDescription('CLTYPE', @UI) cl1 ON cl1.cdCode = base.cltypeCode
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.clId)
		FOR XML RAW)

	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END