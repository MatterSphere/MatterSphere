CREATE TABLE search.ESIndexStructure(
	ESIndexTableId SMALLINT NOT NULL
	, FieldName NVARCHAR(128) NOT NULL
	, ESFieldType NVARCHAR(50) NULL
	, searchable BIT NOT NULL
	, facetable BIT NOT NULL
	, FacetOrder TINYINT NULL
	, Analyzer NVARCHAR(50) NULL
	, IsDefault BIT NOT NULL
	, ExtTable NVARCHAR(128)
	, Suggestable BIT
	, cdCode NVARCHAR(15)
	, fieldCodeLookupGroup NVARCHAR(15) NULL
, CONSTRAINT PK_ESIndexStructure PRIMARY KEY (ESIndexTableId, FieldName)
, CONSTRAINT [FK_ESIndexStructure_EsIndexTable] FOREIGN KEY (ESIndexTableId) REFERENCES search.ESIndexTable (ESIndexTableId) NOT FOR REPLICATION
)
