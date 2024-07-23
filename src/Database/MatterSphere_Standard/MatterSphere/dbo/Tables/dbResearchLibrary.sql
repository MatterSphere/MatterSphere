CREATE TABLE [dbo].[dbResearchLibrary] (
    [ResearchID]  BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [resCategory] [dbo].[uCodeLookup] NOT NULL,
    [resTitle]    NVARCHAR (50)       NOT NULL,
    [resCoretext] NVARCHAR (MAX)      NULL,
    [resSnippet]  NVARCHAR (500)      NULL,
    [resURL]      NVARCHAR (200)      NULL,
    [resPrivate]  BIT                 CONSTRAINT [DF_dbResearchLibrary_resPrivate] DEFAULT ((0)) NOT NULL,
    [Created]     DATETIME            CONSTRAINT [DF_dbResearchLibrary_Created] DEFAULT (getutcdate()) NOT NULL,
    [Updated]     DATETIME            NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbResearchLibrary_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbResearchLibrary] PRIMARY KEY CLUSTERED ([ResearchID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbResearchLibrary_rowguid]
    ON [dbo].[dbResearchLibrary]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbResearchLibrary] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbResearchLibrary] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbResearchLibrary] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbResearchLibrary] TO [OMSApplicationRole]
    AS [dbo];

