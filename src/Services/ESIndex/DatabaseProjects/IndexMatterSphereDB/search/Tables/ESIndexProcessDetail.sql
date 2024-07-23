CREATE TABLE search.ESIndexProcessDetail
(
	ESIndexProcessId INT NOT NULL
	, MessageType NVARCHAR(100) NOT NULL
	, StartDate DATETIME NOT NULL CONSTRAINT DF_ESIndexProcessDetail_StartDate DEFAULT(GETDATE())
	, SuccessNumber INT NULL
	, FailedNumber INT NULL
	, Size BIGINT NULL
	, ProcessTime FLOAT
	, FinishDate DATETIME NULL
	, NumOfProcessedMessages BIGINT
	, ContentReadingFailedNumber INT
	, CONSTRAINT PK_ESIndexProcessDetail PRIMARY KEY CLUSTERED (ESIndexProcessId, MessageType)
	, CONSTRAINT FK_ESIndexProcessDetail_ESIndexProcess_ProcessId FOREIGN KEY (ESIndexProcessId) REFERENCES search.ESIndexProcess(Id)
)
