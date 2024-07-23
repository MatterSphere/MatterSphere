CREATE PROCEDURE search.GetMSNoteAData (@UI uUICultureInfo = '{default}', @BatchSize BIGINT = 1000, @Rel AS search.ESRelationship READONLY, @RelType NVARCHAR(100) = NULL)
AS
SET NOCOUNT ON;
DECLARE @OBJTYPE NVARCHAR(100) = 'NOTE'

DECLARE @CRAWLID INT
	, @PROCNAME NVARCHAR(100) = SUBSTRING(OBJECT_NAME(@@PROCID),4,len(OBJECT_NAME(@@PROCID))-7)
	, @BULKREQUIRED BIT 
	, @COPIEDVERSION BIGINT
	, @WORKINGVERSION BIGINT
	, @appBULKREQUIRED BIT
	, @assocBULKREQUIRED BIT
	, @clBULKREQUIRED BIT
	, @contBULKREQUIRED BIT
	, @fileBULKREQUIRED BIT
	, @tskBULKREQUIRED BIT
	, @TABLENAME NVARCHAR(128)
	, @IndexingEnabled BIT

SELECT 
	@BULKREQUIRED = CASE WHEN changeVersionControl.FullCopyRequired = 1 OR ESIndexTable.FullCopyRequired = 1 THEN 1 ELSE 0 END
	, @COPIEDVERSION = LastCopiedVersion
	, @WORKINGVERSION = WorkingVersion
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = @OBJTYPE

SELECT @appBULKREQUIRED = @BULKREQUIRED
	, @assocBULKREQUIRED = @BULKREQUIRED
	, @clBULKREQUIRED = @BULKREQUIRED
	, @contBULKREQUIRED = @BULKREQUIRED
	, @fileBULKREQUIRED = @BULKREQUIRED
	, @tskBULKREQUIRED = @BULKREQUIRED

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'CLIENT'

IF @IndexingEnabled =  1
BEGIN
	IF @clBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @clBULKREQUIRED = 1
	
	EXEC search.GetMSNoteClientAData @UI, @BatchSize, @clBULKREQUIRED, @COPIEDVERSION
END

SELECT @IndexingEnabled  = NULL, @TABLENAME = NULL

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'CONTACT'

IF @IndexingEnabled =  1
BEGIN
	IF @contBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @contBULKREQUIRED = 1
	
	EXEC search.GetMSNoteContactAData @UI, @BatchSize, @contBULKREQUIRED, @COPIEDVERSION
END


SELECT @IndexingEnabled  = NULL, @TABLENAME = NULL

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'FILE'

IF @IndexingEnabled =  1
BEGIN
	IF @fileBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @fileBULKREQUIRED = 1
	
	EXEC search.GetMSNoteMatterAData @UI, @BatchSize, @fileBULKREQUIRED, @COPIEDVERSION
END

SELECT @IndexingEnabled  = NULL, @TABLENAME = NULL

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'ASSOCIATE'

IF @IndexingEnabled =  1
BEGIN
	IF @assocBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @assocBULKREQUIRED = 1
	
	EXEC search.GetMSNoteAssociateAData @UI, @BatchSize, @assocBULKREQUIRED, @COPIEDVERSION
END

SELECT @IndexingEnabled  = NULL, @TABLENAME = NULL

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'APPOINTMENT'

IF @IndexingEnabled =  1
BEGIN
	IF @appBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @appBULKREQUIRED = 1
	
	EXEC search.GetMSNoteAppointmentAData @UI, @BatchSize, @appBULKREQUIRED, @COPIEDVERSION
END

SELECT @IndexingEnabled  = NULL, @TABLENAME = NULL

SELECT
	@IndexingEnabled  = IndexingEnabled
	, @TABLENAME = tablename
FROM search.ESIndexTable CROSS APPLY search.ChangeVersionControl 
WHERE ObjectType = 'TASK'

IF @IndexingEnabled =  1
BEGIN
	IF @tskBULKREQUIRED = 0
		IF @COPIEDVERSION < CHANGE_TRACKING_MIN_VALID_VERSION( OBJECT_ID(@TABLENAME))
			SET @tskBULKREQUIRED = 1
	
	EXEC search.GetMSNoteTaskAData @UI, @BatchSize, @tskBULKREQUIRED, @COPIEDVERSION
END
