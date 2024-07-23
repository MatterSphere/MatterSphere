CREATE PROCEDURE search.GetMSAppointmentAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Appointment'
	, @X XML
	, @appID BIGINT
	, @TABLENAME NVARCHAR(100) = 'dbo.dbAppointments'

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
--DECLARE @T1 AS search.ESUGPD

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT

DECLARE @UGDP NVARCHAR(100) = (SELECT CAST(ID AS NVARCHAR(40)) + '(ALLOW)' FROM item.[Group] WHERE Name = 'AllInternal')

SELECT 
	@BULKREQUIRED = CASE WHEN cvc.FullCopyRequired = 1 OR IE.FullCopyRequired = 1 THEN 1 ELSE 0 END
	, @COPIEDVERSION = LastCopiedVersion
	, @WORKINGVERSION = WorkingVersion
FROM search.IndexedEntity IE
	CROSS APPLY search.ChangeVersionControl  CVC
WHERE IE.EntityName = @EntityName

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
		, CT.SYS_CHANGE_OPERATION AS Op
	FROM CHANGETABLE(CHANGES dbo.dbAppointments, @COPIEDVERSION) AS CT

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END
SET @appID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @appID IS NOT NULL
BEGIN
/*
	INSERT INTO @T1
	SELECT DISTINCT base.fileID 
	FROM @TBatch tb
		INNER JOIN dbo.dbAppointments base ON base.appID = tb.Id
*/		
	SET @X = (
		SELECT base.appId AS [mattersphereid]
			, base.fileID AS [file-id]
			, base.clID AS [client-id]
			, base.AssocID AS [associate-id]
			, base.docID as [document-id]
			, COALESCE(cl1.cdDesc, '~' + base.appType + '~') AS [appointmentType]
			, dbo.RemoveIllegalXMLCharacters(ISNULL(base.appDesc, '')) AS appDesc
			, base.appLocation
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, @UGDP AS [ugdp]--CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM dbo.dbAppointments base
			LEFT JOIN dbo.GetCodeLookupDescription('APPTYPE', @UI) cl1 ON cl1.cdCode = base.appType
--			LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
			INNER JOIN @TBatch tb ON tb.Id = base.appId
		FOR XML RAW
		)

	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch
	--DELETE @T1

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
		ORDER BY Id
	
	SET @appID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
