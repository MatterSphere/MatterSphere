CREATE PROCEDURE search.GetMSPrecedentAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'PRECEDENT'
	, @X XML
	, @precID BIGINT
	, @RelFlag BIT

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @RelD AS search.ESRelationship
DECLARE @RelC AS search.ESRelationship

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT
	, @TABLENAME NVARCHAR(128)
	, @ReindexFailedItems BIT
	, @ProcessOrder BIT

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
		, @ReindexFailedItems = CASE ReindexFailedItems WHEN 2 THEN 1 ELSE 0 END
		, @ProcessOrder = ProcessOrder
	FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
	WHERE ObjectType = @OBJTYPE

	IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
		SET @BULKREQUIRED = 1

	IF @BULKREQUIRED = 1
		IF @ProcessOrder = 0
			INSERT INTO @TBatch(Id, Op)
			SELECT 
				precID
				, 'I'
			FROM dbo.dbPrecedents
			ORDER BY precID
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP (@BatchSize)
				precID
				, 'I'
			FROM dbo.dbPrecedents
			ORDER BY precID DESC
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
												OR (dbPrecedents.PrecSubCategory = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'PRECSUBCAT')
												OR (dbPrecedents.PrecMinorCategory = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'PRECMINORCAT'))
			UNION
			SELECT p.precID
				, 'U'
			FROM dbo.dbPrecedents AS p
				INNER JOIN search.ExtensionToReindex AS e ON e.Extension = p.precExtension AND E.Flag = 1
			UNION
			SELECT p.precID
				, 'U'
			FROM dbo.dbPrecedents AS p
				INNER JOIN search.ESIndexPrecedentLog AS R ON R.precID = p.precId AND R.ErrB = 1 AND @ReindexFailedItems = 1
			) r
		GROUP BY r.precID

		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		ORDER BY Id, Op

		SET @ProcessOrder = 0
	END
	IF @ProcessOrder = 0
		SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
	ELSE
		SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id)

	WHILE @precID IS NOT NULL
	BEGIN
		SET @X = (
			SELECT base.rowguid AS [id]
				, base.PrecTitle AS [precTitle]
				, COALESCE(cl1.cdDesc, '~' + base.prectype + '~') AS [precedentType]
				, COALESCE(cl2.cdDesc, '~' + base.PrecLibrary + '~') AS [precLibrary]
				, COALESCE(cl3.cdDesc, '~' + base.PrecCategory + '~') AS  [precCategory]
				, COALESCE(cl4.cdDesc, '~' + base.PrecSubCategory + '~') AS [precSubCategory]
				, COALESCE(cl5.cdDesc, '~' + base.PrecMinorCategory + '~') AS [precMinorCategory]
				, dbo.RemoveIllegalXMLCharacters(base.precDesc) AS [precDesc]
				, '' AS [docContents]
				, CASE base.precDeleted WHEN 1 THEN 'true' ELSE 'false' END AS [precDeleted]
				, LOWER(REPLACE(base.precExtension, '.', '')) AS [precedentExtension]
				, ISNULL(base.Updated, base.Created) AS [modifieddate]
				, base.PrecTitle AS [title]
				, base.precID AS [mattersphereid]
				, ISNULL(dir.dirPath, dir1.dirPath) + '\' + base.PrecPath AS [Sys_FileName]
				, @UGDP AS [ugdp]
				/**** CUSTOM COLUMNS BEGIN *****/
				/**** CUSTOM COLUMNS END *****/
			FROM [dbo].[dbPrecedents] base
				CROSS JOIN dbo.dbreginfo r
				LEFT OUTER JOIN dbo.dbDirectory dir ON base.PrecDirID IS NOT NULL AND dir.dirID = base.PrecDirID 
				LEFT OUTER JOIN dbo.dbDirectory dir1 ON base.PrecDirID IS NOT NULL AND r.BRID = dir1.BRID AND dir1.DIRCODE = 'OMPRECEDENTS'
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('doctype', @UI) cl1 ON cl1.cdCode = prectype
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECLIBRARY', @UI) cl2 ON cl2.cdCode = [PrecLibrary]
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('PrecCat', @UI) cl3 ON cl3.cdCode = [PrecCategory]
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECSUBCAT', @UI) cl4 ON cl4.cdCode = [PrecSubCategory]
				LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECMINORCAT', @UI) cl5 ON cl5.cdCode = [PrecMinorCategory]
				/**** CUSTOM EXTDATA BEGIN *****/
				/**** CUSTOM EXTDATA END *****/
			WHERE EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = base.precID)
			FOR XML RAW
			)

		SET @X = (SELECT @OBJTYPE AS [@objecttype], @X FOR XML PATH('root'))
		EXEC search.SendESIndexMessage @X;

		/**** CUSTOM PARENT BEGIN *****/
		/**** CUSTOM PARENT END *****/

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

		IF @BULKREQUIRED = 1
			IF @ProcessOrder = 0
				INSERT INTO @TBatch(Id, Op)
				SELECT TOP (@BatchSize)
					precID
					, 'I'
				FROM dbo.dbPrecedents
				WHERE precID > @precID
				ORDER BY precID
			ELSE
				INSERT INTO @TBatch(Id, Op)
				SELECT TOP (@BatchSize)
					precID
					, 'I'
				FROM dbo.dbPrecedents
				WHERE precID < @precID
				ORDER BY precID DESC
		ELSE
			INSERT INTO @TBatch(Id, Op)
			SELECT TOP(@BatchSize)
				Id
				, Op
			FROM @TInc
			WHERE Id > @precID
			ORDER BY Id, Op

		IF @ProcessOrder = 0
			SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
		ELSE
			SET @precID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id)
	END
END
ELSE 
BEGIN 
	SET @X = (
		SELECT base.rowguid AS [id]
			, base.PrecTitle AS [precTitle]
			, COALESCE(cl1.cdDesc, '~' + base.prectype + '~') AS [precedentType]
			, COALESCE(cl2.cdDesc, '~' + base.PrecLibrary + '~') AS [precLibrary]
			, COALESCE(cl3.cdDesc, '~' + base.PrecCategory + '~') AS  [precCategory]
			, COALESCE(cl4.cdDesc, '~' + base.PrecSubCategory + '~') AS [precSubCategory]
			, COALESCE(cl5.cdDesc, '~' + base.PrecMinorCategory + '~') AS [precMinorCategory]
			, dbo.RemoveIllegalXMLCharacters(base.precDesc) AS [precDesc]
			, '' AS [docContents]
			, CASE base.precDeleted WHEN 1 THEN 'true' ELSE 'false' END AS [precDeleted]
			, LOWER(REPLACE(base.precExtension, '.', '')) AS [precedentExtension]
			/**** CUSTOM COLUMNS BEGIN *****/
			/**** CUSTOM COLUMNS END *****/
			, CAST((SELECT CAST(Id AS NVARCHAR(40)) + ',' FROM @Rel AS r WHERE r.RelPK = base.precID FOR XML PATH(''), TYPE) AS NVARCHAR(MAX)) AS [Sys_Rel]
		FROM [dbo].[dbPrecedents] base 
			CROSS JOIN dbo.dbreginfo r
			LEFT OUTER JOIN dbo.dbDirectory dir ON base.PrecDirID IS NOT NULL AND dir.dirID = base.PrecDirID 
			LEFT OUTER JOIN dbo.dbDirectory dir1 ON base.PrecDirID IS NOT NULL AND r.BRID = dir1.BRID AND dir1.DIRCODE = 'OMPRECEDENTS'
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('doctype', @UI) cl1 ON cl1.cdCode = prectype
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECLIBRARY', @UI) cl2 ON cl2.cdCode = [PrecLibrary]
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PrecCat', @UI) cl3 ON cl3.cdCode = [PrecCategory]
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECSUBCAT', @UI) cl4 ON cl4.cdCode = [PrecSubCategory]
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('PRECMINORCAT', @UI) cl5 ON cl5.cdCode = [PrecMinorCategory]
			/**** CUSTOM EXTDATA BEGIN *****/
			/**** CUSTOM EXTDATA END *****/
		WHERE EXISTS(SELECT 1 FROM @Rel AS r WHERE r.RelPK = base.precID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], @RelType AS [@relatedtype], @X FOR XML PATH('root'))

	EXEC search.SendESIndexMessage @X;
END
