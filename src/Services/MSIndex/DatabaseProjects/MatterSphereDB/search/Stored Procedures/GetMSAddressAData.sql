CREATE PROCEDURE search.GetMSAddressAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Address'
	, @X XML
	, @addId BIGINT
	, @TABLENAME NVARCHAR(100) = 'dbo.dbAddress'

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

SELECT 
	@BULKREQUIRED = FullCopyRequired
	, @COPIEDVERSION = LastCopiedVersion
	, @WORKINGVERSION = WorkingVersion
FROM search.ChangeVersionControl 

IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
	SET @BULKREQUIRED = 1

IF @BULKREQUIRED = 1
	INSERT INTO @TBatch(Id, Op)
	SELECT TOP (@BatchSize)
		addId
		, 'I'
	FROM dbo.dbAddress
	ORDER BY addId
ELSE
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
	ORDER BY Id
END
SET @addId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @addId IS NOT NULL
BEGIN
	SET @X = (
		SELECT 	base.addId AS [mattersphereid]
			, ISNULL(base.addLine1, '') + ' ' + ISNULL(base.addLine2, '') + ' ' + ISNULL(base.addLine3, '') + ' ' + ISNULL(base.addLine4, '') + ' ' + ISNULL(base.addLine5, '') + ' ' + ISNULL(base.addPostcode, '') AS [sc]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, @UGDP AS [ugdp]
			, tb.Op AS [op]
		FROM dbo.dbAddress base
			INNER JOIN @TBatch tb ON tb.Id = base.addId
		FOR XML RAW
		)

	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			addId
			, 'I'
		FROM dbo.dbAddress
		WHERE addId > @addId
		ORDER BY addId
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @addId
		ORDER BY Id

		SET @addId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

END
