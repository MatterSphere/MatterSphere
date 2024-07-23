CREATE PROCEDURE search.GetMSUserAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @OBJTYPE NVARCHAR(100) = 'USERS'
	, @X XML
	, @usrID BIGINT

DECLARE @TBatch TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))
DECLARE @TInc TABLE (Id BIGINT PRIMARY KEY, Op NCHAR(1))

DECLARE @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT
	, @TABLENAME NVARCHAR(128)


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
		usrID
		, 'I'
	FROM dbo.dbUser
	WHERE AccessType = 'INTERNAL'
	ORDER BY usrID
ELSE
BEGIN
	INSERT INTO @TInc(Id, Op)
	SELECT 
		usrId
		, CT.SYS_CHANGE_OPERATION
	FROM CHANGETABLE(CHANGES dbo.dbUser, @COPIEDVERSION) AS [CT]

	INSERT INTO @TBatch(Id, Op)
	SELECT TOP(@BatchSize)
		Id
		, Op
	FROM @TInc
	ORDER BY Id, Op
END

SET @usrID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)

WHILE @usrID IS NOT NULL
BEGIN
	SET @X = (
		SELECT iu.ID AS [id]
			, u.usrID AS [mattersphereid]
			, u.usrInits AS [usrinits]
			, u.usrAlias AS [usralias]
			, u.usrADID AS [usrad]
			, u.usrSQLID AS [usrsql]
			, u.usrFullName AS [usrfullname]
			, CASE WHEN u.usrActive = 1 THEN 'ACTIVE' ELSE 'INACTIVE' END AS [usractive]
			, ISNULL(u.Updated, u.Created) AS [modifieddate]
			, (SELECT STUFF((SELECT ' ' + CAST(ID AS NVARCHAR(MAX)) FROM config.GetUserAndGroupMembershipNT (u.usrADID) FOR XML PATH (''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) AS [usrAccessList]
		FROM dbo.dbUser u
			INNER JOIN item.[User] iu ON iu.NTLogin = u.usrADID
		WHERE u.AccessType = 'INTERNAL'
			AND EXISTS(SELECT 1 FROM @TBatch tb WHERE tb.Id = u.usrID)
		FOR XML RAW
		)
	
	SET @X = (SELECT @OBJTYPE AS [@objecttype], 'MatterSphere ' AS [@sourcesystem], @X FOR XML PATH('root'))
	EXEC search.SendESIndexMessage @X;

	DELETE @TBatch

	IF @BULKREQUIRED = 1
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP (@BatchSize)
			usrID
			, 'I'
		FROM dbo.dbUser
		WHERE AccessType = 'INTERNAL'
			AND usrID > @usrID
		ORDER BY usrID
	ELSE
		INSERT INTO @TBatch(Id, Op)
		SELECT TOP(@BatchSize)
			Id
			, Op
		FROM @TInc
		WHERE Id > @usrID
		ORDER BY Id, Op

	SET @usrID = (SELECT TOP 1 Id FROM @TBatch ORDER BY Id DESC)
END