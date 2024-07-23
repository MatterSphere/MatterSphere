CREATE PROCEDURE search.GetMSPrecedentAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Precedent'
	, @X XML
	, @precID BIGINT
	, @TABLENAME NVARCHAR(100) = 'dbo.Precedent'

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))

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
		precID
		, 'I'
	FROM dbo.dbPrecedents
	ORDER BY precID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT TOP (@BatchSize)
		precID
		, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
	FROM (
		SELECT TOP(@BatchSize)
			precID
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE(CHANGES dbo.dbPrecedents, @COPIEDVERSION) AS CT
		UNION
		SELECT dbPrecedents.precID 
			, 'U'
		FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
			INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
			INNER JOIN dbo.dbPrecedents ON ((dbPrecedents.prectype = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'SUBASSOC')
											OR (dbPrecedents.PrecLibrary = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'PRECLIBRARY')
											OR (dbPrecedents.PrecCategory = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'PrecCat')
											OR (dbPrecedents.PrecSubCategory = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'PRECSUBCAT'))
		) r
	GROUP BY r.precID

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id

END

SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @precID IS NOT NULL
BEGIN

	SET @X = (
		SELECT base.precID AS [mattersphereid]
			, COALESCE(cl1.cdDesc, '~' + base.prectype + '~') AS [precedentType]
			, COALESCE(cl3.cdDesc, '~' + base.PrecCategory + '~') AS  [precCategory]
			, base.precDeleted AS [precDeleted]
			, LOWER(REPLACE(base.precExtension, '.', '')) AS [precedentExtension]
			, dbo.RemoveIllegalXMLCharacters(base.precDesc) AS [precDesc]
			, dbo.RemoveIllegalXMLCharacters(base.precTitle) AS [precTitle]
			, COALESCE(cl2.cdDesc, '~' + base.precLibrary + '~') AS [precLibrary]
			, COALESCE(cl4.cdDesc, '~' + base.precSubCategory + '~') AS [precSubCategory]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, @UGDP AS [ugdp]
			, tb.Op AS [op]
		FROM [dbo].[dbPrecedents] base
			CROSS JOIN dbo.dbreginfo r
			LEFT OUTER JOIN dbo.dbDirectory dir ON base.PrecDirID IS NOT NULL AND dir.dirID = base.PrecDirID 
			LEFT OUTER JOIN dbo.dbDirectory dir1 ON base.PrecDirID IS NOT NULL AND r.BRID = dir1.BRID AND dir1.DIRCODE = 'OMPRECEDENTS'
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('doctype', @UI) cl1 ON cl1.cdCode = prectype
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECLIBRARY', @UI) cl2 ON cl2.cdCode = [PrecLibrary]
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PrecCat', @UI) cl3 ON cl3.cdCode = [PrecCategory]
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECSUBCAT', @UI) cl4 ON cl4.cdCode = [PrecSubCategory]
			INNER JOIN @TBatch tb ON tb.Id = base.precID
		FOR XML RAW
		)
		
	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			precID
			, 'I'
		FROM dbo.dbPrecedents
		WHERE precID > @precID
		ORDER BY precID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @precID
		ORDER BY Id

	SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
