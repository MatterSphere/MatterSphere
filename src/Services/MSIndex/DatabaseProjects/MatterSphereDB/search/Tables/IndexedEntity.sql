CREATE TABLE search.IndexedEntity (
	EntityName NVARCHAR(50) NOT NULL
	, FullCopyRequired BIT NULL DEFAULT 1
	, CONSTRAINT PK_IndexedEntity PRIMARY KEY CLUSTERED (EntityName)
);

