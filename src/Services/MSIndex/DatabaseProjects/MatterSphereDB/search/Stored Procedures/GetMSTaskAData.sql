CREATE PROCEDURE search.GetMSTaskAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Task'
	, @X XML
	, @tskID BIGINT
	, @TABLENAME NVARCHAR(100) = 'dbo.dbTasks'

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
		UNION
		SELECT dbTasks.tskID 
			, 'U'
		FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
			INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
			INNER JOIN dbo.dbTasks ON (dbTasks.tskType = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'TASKTYPE')
		) r
	GROUP BY r.tskID

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
/*	INSERT INTO @T1
	SELECT DISTINCT base.fileID 
	FROM @TBatch tb
		INNER JOIN dbo.dbTasks base ON base.tskID = tb.Id
*/
	SET @X = (
		SELECT base.tskID AS [mattersphereid]
			, base.fileID AS [file-id]
			, base.docID as [document-id]
			, COALESCE(cl1.cdDesc, '~' + base.tskType + '~') AS [taskType]
			, dbo.RemoveIllegalXMLCharacters(base.tskDesc) AS [tskDesc]
			, dbo.RemoveIllegalXMLCharacters(base.tskNotes) AS [tskNotes]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, @UGDP AS [ugdp] --CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM dbo.dbTasks base
			LEFT JOIN dbo.GetCodeLookupDescription('TASKTYPE', @UI) cl1 ON cl1.cdCode = base.tskType
--			LEFT JOIN search.FormatMSPermForAS('FILE', @T1) obj ON obj.ID = base.fileID
			INNER JOIN @TBatch tb ON tb.Id = base.tskID
		FOR XML RAW
		)
		
	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch
	--DELETE @T1

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
		ORDER BY Id

	SET @tskID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
