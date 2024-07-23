CREATE TABLE search.ExtensionToReindex
(
	Extension NVARCHAR(15) CONSTRAINT PK_ExtensionToReindex PRIMARY KEY
	, Flag BIT DEFAULT (0)
)
