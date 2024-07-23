CREATE PROCEDURE search.GetMSCCLinkAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'CCLINK'
	, @X XML
	, @ID BIGINT
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
			ID
			, 'I'
		FROM dbo.dbClientContacts
		ORDER BY ID
	ELSE
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			ID
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE(CHANGES dbo.dbClientContacts, @COPIEDVERSION) AS CT

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op
	END

	SET @ID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

	WHILE @ID IS NOT NULL
	BEGIN
		INSERT INTO @T1
		SELECT DISTINCT base.clID 
		FROM @TBatch tb
			INNER JOIN dbo.dbClientContacts base ON base.ID = tb.Id

		SET @X = (
			SELECT base.rowguid AS [id]				  
				, base.clID AS [client-id]
				, base.contID AS [contact-id]					  
				, base.ID AS [mattersphereid]
				, CAST(NULL AS DATETIME) AS [modifieddate] 
				, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
	            /**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM dbo.dbClientContacts base
				LEFT JOIN search.FormatMSPermForAS('CLIENT', @T1) obj ON obj.ID = base.clID
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.ID)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		INSERT INTO @RelD (Id, RelPK)
		SELECT p.rowguid
			, p.clID
		FROM @TBatch rb
			INNER JOIN dbo.dbClientContacts p ON p.ID = rb.Id
		WHERE rb.Op = 'I'
	
		IF @@ROWCOUNT > 0
		BEGIN
			EXEC search.GetMSClientAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

			DELETE @RelD

			INSERT INTO @RelD (Id, RelPK)
			SELECT p.rowguid
				, p.contID
			FROM @TBatch rb
				INNER JOIN dbo.dbClientContacts p ON p.ID = rb.Id
			WHERE rb.Op = 'I'

			EXEC search.GetMSContactAData @UI = @UI, @Rel = @RelD, @RelType = @OBJTYPE 

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
				ID
				, 'I'
			FROM dbo.dbClientContacts
			WHERE ID > @ID
			ORDER BY ID
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP(@BatchSize)
				Id
				, Op
			FROM @TInc
			WHERE Id > @ID
			ORDER BY Id, Op
			
		SET @ID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	END
END
ELSE 
BEGIN 
	INSERT INTO @T1
	SELECT DISTINCT base.clId
	FROM @Rel tb
		INNER JOIN dbo.dbClientContacts base ON base.ID = tb.RelPK

	SET @X = (
		SELECT base.rowguid AS [id]				  
			, base.clID AS [client-id]
			, base.contID AS [contact-id]					  
	        /**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.ID FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM dbo.dbClientContacts base
			LEFT JOIN search.FormatMSPermForAS('CLIENT', @T1) obj ON obj.ID = base.clID
		/**** CUSTOM EXTDATA BEGIN *****/
		/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.ID)
		FOR XML RAW
		)
	
	EXEC search.SendESIndexMessage @X;
END
