CREATE TABLE search.ESIndexCommonStructure
(
	ESIndexId SMALLINT NOT NULL
	, FieldName NVARCHAR(128) NOT NULL
	, ESFieldType NVARCHAR(50) NULL
	, searchable BIT NOT NULL
	, facetable BIT NOT NULL
	, FacetOrder TINYINT NULL
	, Analyzer NVARCHAR(50) NULL
	, cdCode NVARCHAR(15)
, CONSTRAINT PK_ESIndexCommonStructure PRIMARY KEY (ESIndexId, FieldName)
, CONSTRAINT FK_ESIndexCommonStructure_EsIndex FOREIGN KEY (ESIndexId) REFERENCES search.ESIndex (ESIndexId) NOT FOR REPLICATION
)
