CREATE PROCEDURE search.GetMSClientAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Client'
	, @X XML
	, @clID BIGINT
	, @TABLENAME NVARCHAR(100) = 'config.dbClient'

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @T1 AS search.ESUGPD

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

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END

SET @clID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @clID IS NOT NULL
BEGIN

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT base.clID AS [mattersphereid]
			, base.clDefaultAddress AS [address-id]
			, COALESCE(cl1.cdDesc, '~' + base.cltypeCode + '~') AS [clientType]
			, dbo.RemoveIllegalXMLCharacters(base.clName) AS [clName]
			, base.clNo AS [clNo]
			, base.clNotes AS [clNotes]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM config.dbClient base
			LEFT JOIN dbo.GetCodeLookupDescription('CLTYPE', @UI) cl1 ON cl1.cdCode = base.cltypeCode
			LEFT JOIN search.FormatMSPermForAS(@EntityName, @T1) obj ON obj.ID = base.clID
			INNER JOIN @TBatch tb ON tb.Id = base.clID
		FOR XML RAW
		)

	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

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
		ORDER BY Id

	SET @clID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

END
