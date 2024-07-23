CREATE PROCEDURE search.GetMSAddressAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'ADDRESS'
	, @X XML
	, @addId BIGINT
	, @RelFlag BIT

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @RelD AS search.ESRelationship
DECLARE @RelC AS search.ESRelationship

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT
	, @TABLENAME NVARCHAR(100)

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

IF @RelType IS NULL
	SET @RelFlag = 0
ELSE 
	SET @RelFlag = 1

IF @RelFlag = 0
BEGIN 
	SELECT 
		@BULKREQUIRED = FullCopyRequired
		, @COPIEDVERSION = LastCopiedVersion
		, @WORKINGVERSION = WorkingVersion
	FROM search.ChangeVersionControl 


	IF @BULKREQUIRED = 0
	BEGIN
		INSERT INTO @TInc(Id, Op)
		SELECT
			addId
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE(CHANGES dbo.dbAddress, @COPIEDVERSION) AS CT

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op
	END
	SET @addId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

	WHILE @addId IS NOT NULL
	BEGIN

		IF EXISTS(SELECT 1 FROM @TBatch)
		BEGIN
			INSERT INTO @RelC (Id, RelPK)
			SELECT c.rowguid
				, rb.Id
			FROM @TBatch rb
				INNER JOIN config.dbContact c ON c.contDefaultAddress = rb.Id

			IF @@ROWCOUNT > 0
				EXEC search.GetMSAddressAData @UI = @UI, @Rel = @RelC, @RelType = 'CONTACT'

			DELETE @RelC

			INSERT INTO @RelC (Id, RelPK)
			SELECT c.rowguid
				, rb.Id
			FROM @TBatch rb
				INNER JOIN config.dbClient c ON c.clDefaultAddress = rb.Id

			IF @@ROWCOUNT > 0
				EXEC search.GetMSAddressAData @UI = @UI, @Rel = @RelC, @RelType = 'CLIENT'

			DELETE @RelC
			/**** CUSTOM CHILD BEGIN *****/
			/**** CUSTOM CHILD END *****/
		END

		DELETE @TBatch
		DELETE @RelD

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @addId
		ORDER BY Id, Op

		SET @addId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	END
END
ELSE 
BEGIN 
	SET @X = (
		SELECT base.rowguid AS [id]
			, dbo.GetAddress(base.addID, ', ', @UI) AS [address]
			, base.addPostcode AS [addPostcode]
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.addId FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM dbo.dbAddress base
		/**** CUSTOM EXTDATA BEGIN *****/
		/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.addId)
		FOR XML RAW
		)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@RelatedType], @X FOR XML PATH('root'))
	
	EXEC search.SendESIndexMessage @X;
END
