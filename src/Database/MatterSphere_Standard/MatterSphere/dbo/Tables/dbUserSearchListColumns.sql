CREATE TABLE dbo.dbUserSearchListColumns
(
	NTLogin NVARCHAR(200) NOT NULL
	, schCode dbo.uCodeLookup NOT NULL
	, schListView dbo.uXML
	, rowguid UNIQUEIDENTIFIER CONSTRAINT [DF_dbUserSearchListColumns_rowguid] DEFAULT NEWID() ROWGUIDCOL NOT NULL
	, CONSTRAINT PK_dbUserSearchListColumns PRIMARY KEY CLUSTERED (NTLogin, schCode)
)
