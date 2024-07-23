CREATE TYPE search.IndexingInfo AS TABLE(
	 EntityID BIGINT PRIMARY KEY
	, Sys_FileName NVARCHAR(MAX)
	, Sys_FileSize BIGINT
	, Sys_ProcessTime FLOAT
	, Sys_DocIndexingError NVARCHAR(MAX)
	, Sys_ErrorCode NVARCHAR(MAX)
	, docExtension NVARCHAR(15)
	, EmptyContent BIT

)