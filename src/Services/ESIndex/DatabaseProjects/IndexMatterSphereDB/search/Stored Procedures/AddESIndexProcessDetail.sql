CREATE PROCEDURE search.AddESIndexProcessDetail
	@MessageType NVARCHAR(100)
	, @SuccessNumber INT = NULL
	, @FailedNumber INT = NULL
	, @Size BIGINT = NULL
	, @ProcessTime FLOAT = 0
	, @NumOfProcessedMessages BIGINT = 0
	, @ContentReadingFailedNumber INT = 0

AS
SET NOCOUNT ON
DECLARE @LastIndexProcessID INT 
	, @FinishDate DATETIME

SELECT TOP 1 @LastIndexProcessID = Id
	, @FinishDate = FinishDate
FROM search.ESIndexProcess 
ORDER BY Id DESC

IF @LastIndexProcessID IS NULL
BEGIN
	RAISERROR( N'Index process has not yet start', 11,  1);
	RETURN;
END;

IF @FinishDate IS NOT NULL
BEGIN
	RAISERROR( N'Index process already finished', 11,  1);
	RETURN;
END;

MERGE INTO search.ESIndexProcessDetail WITH (HOLDLOCK) t
USING (SELECT @LastIndexProcessID, @MessageType, @SuccessNumber, @FailedNumber, @Size, @ProcessTime, @NumOfProcessedMessages, @ContentReadingFailedNumber) s (ESIndexProcessId, MessageType, SuccessNumber, FailedNumber, Size, ProcessTime, NumOfProcessedMessages, ContentReadingFailedNumber)
	ON s.ESIndexProcessId = t.ESIndexProcessId
		AND s.MessageType = t.MessageType
WHEN NOT MATCHED BY TARGET
	THEN INSERT (ESIndexProcessId, MessageType, SuccessNumber, FailedNumber, Size, FinishDate, ProcessTime, NumOfProcessedMessages, ContentReadingFailedNumber)
	VALUES (s.ESIndexProcessId, s.MessageType, s.SuccessNumber, s.FailedNumber, s.Size, GETDATE(), s.ProcessTime, s.NumOfProcessedMessages, s.ContentReadingFailedNumber)
WHEN MATCHED 
	THEN UPDATE
		SET SuccessNumber = ISNULL(t.SuccessNumber, 0) + ISNULL(s.SuccessNumber, 0)
		, FailedNumber = ISNULL(t.FailedNumber, 0) + ISNULL(s.FailedNumber, 0)
		, Size = ISNULL(t.Size, 0) + ISNULL(s.Size, 0)
		, FinishDate = GETDATE()
		, ProcessTime = ISNULL(t.ProcessTime, 0) + ISNULL(s.ProcessTime, 0)
		, NumOfProcessedMessages = ISNULL(t.NumOfProcessedMessages, 0) + ISNULL(s.NumOfProcessedMessages, 0)
		, ContentReadingFailedNumber = ISNULL(t.ContentReadingFailedNumber, 0) + ISNULL(s.ContentReadingFailedNumber, 0);
