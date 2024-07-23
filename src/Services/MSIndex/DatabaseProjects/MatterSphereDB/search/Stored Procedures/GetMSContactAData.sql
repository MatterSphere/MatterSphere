CREATE PROCEDURE search.GetMSContactAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Contact'
	, @X XML
	, @contID BIGINT
	, @TABLENAME NVARCHAR(100) = 'config.dbContact'

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

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END

SET @contID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @contID IS NOT NULL
BEGIN

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT base.contID AS [mattersphereid]
			, COALESCE(cl1.cdDesc, '~' + base.contTypeCode + '~') AS [contactType]
			, base.contDefaultAddress as [address-id]
			, dbo.RemoveIllegalXMLCharacters(base.contName) AS [contName]
			, dbo.RemoveIllegalXMLCharacters(base.contNotes) AS [contNotes]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM config.dbContact base
			LEFT OUTER JOIN dbo.dbAddress a on a.addID = base.contDefaultAddress
			LEFT OUTER JOIN dbo.GetCodeLookupDescription('CONTTYPE', @UI) cl1 ON cl1.cdCode = base.contTypeCode
			LEFT JOIN search.FormatMSPermForAS(@EntityName, @T1) obj ON obj.ID = base.contId
			INNER JOIN @TBatch tb ON tb.Id = base.contID
		FOR XML RAW
		)

	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

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
		ORDER BY Id

	SET @contID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END
