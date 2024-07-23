CREATE TABLE [dbo].[dbPrecedentKeys]
(
	precKey NVARCHAR(30)
	, KeySection TINYINT
	, cltypeCode dbo.uCodeLookup NULL
	, assocType dbo.uCodeLookup NULL
	, listType dbo.uCodeLookup NULL
	, precKeyMessage NVARCHAR(MAX)
	, CONSTRAINT PK_dbPrecedentKeys PRIMARY KEY CLUSTERED (precKey, KeySection)
)

GO
CREATE UNIQUE INDEX UX_dbPrecedentKeys_KeySection_precKey ON [dbo].[dbPrecedentKeys] (KeySection, precKey)