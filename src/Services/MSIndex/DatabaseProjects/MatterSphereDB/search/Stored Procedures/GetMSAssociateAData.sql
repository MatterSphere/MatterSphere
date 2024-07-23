CREATE PROCEDURE search.GetMSAssociateAData(@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000)
AS

SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @EntityName NVARCHAR(50) = 'Associate'
	, @X XML
	, @assocId BIGINT
	, @TABLENAME NVARCHAR(100) = 'config.dbAssociates'

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
		assocId
		, 'I'
	FROM config.dbAssociates
	ORDER BY assocId
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT
		assocId
		, MAX(CASE Op WHEN 'I' THEN 'I' ELSE 'U' END)
	FROM (
		SELECT assocID
			, CT.SYS_CHANGE_OPERATION AS Op
		FROM CHANGETABLE( CHANGES config.dbAssociates, @COPIEDVERSION) AS CT
		UNION
		SELECT dbAssociates.assocID 
			, 'U'
		FROM CHANGETABLE( CHANGES relationship.UserGroup_Contact, @COPIEDVERSION) AS [CT]
			LEFT OUTER JOIN relationship.UserGroup_Contact ON CT.RelationshipID = UserGroup_Contact.RelationshipID
			LEFT OUTER JOIN relationship.Usergroup_Contact_Delete ON CT.RelationshipID = Usergroup_Contact_Delete.ID
			INNER JOIN config.dbAssociates ON dbAssociates.contID = ISNULL(UserGroup_Contact.ContactID, Usergroup_Contact_Delete.ContactID)
		UNION
		SELECT dbAssociates.assocID 
			, 'U'
		FROM CHANGETABLE(CHANGES dbo.dbCodeLookup, @COPIEDVERSION) AS CT
			INNER JOIN dbo.dbCodeLookup ON CT.cdID = dbCodeLookup.cdID 
			INNER JOIN config.dbAssociates ON (dbAssociates.assoctype = dbCodeLookup.cdCode AND dbCodeLookup.cdType = 'SUBASSOC')
		) r
	GROUP BY assocId

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id
END

SET @assocId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @assocId IS NOT NULL
BEGIN

	INSERT INTO @T1
	SELECT ID 
	FROM @TBatch

	SET @X = (
		SELECT base.assocId AS [mattersphereid]
			, base.fileID AS [file-id]
			, base.contID AS [contact-id]
			, COALESCE(cl1.cdDesc, '~' + base.assoctype + '~') AS [associateType]
			, dbo.RemoveIllegalXMLCharacters(base.assocHeading) [assocHeading]
			, base.assocSalut AS [assocSalut]
			, base.assocAddressee AS [assocAddressee]
			, dbo.RemoveIllegalXMLCharacters(base.assocNotes) AS [assocNotes]
			, ISNULL(base.Updated, base.Created) AS [modifieddate]
			, CASE WHEN obj.UGDP IS NULL THEN @UGDP ELSE obj.UGDP END AS [ugdp]
			, tb.Op AS [op]
		FROM config.dbAssociates base
			LEFT JOIN [dbo].[GetCodeLookupDescription]('SUBASSOC', @UI) cl1 ON cl1.cdCode = base.assoctype
			LEFT JOIN search.FormatMSPermForAS(@EntityName, @T1) obj ON obj.ID = base.assocId
			INNER JOIN @TBatch tb ON tb.Id = base.assocId
		FOR XML RAW
		)

	SET @X = (SELECT @EntityName AS [@entityname], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch
	DELETE @T1

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			assocId
			, 'I'
		FROM config.dbAssociates
		WHERE assocId > @assocId
		ORDER BY assocId
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @assocId
		ORDER BY Id
	
	SET @assocId = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

END
