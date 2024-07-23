CREATE TABLE [dbo].[dbScript] (
    [scrCode]    [dbo].[uCodeLookup] NOT NULL,
    [scrType]    [dbo].[uCodeLookup] CONSTRAINT [DF_dbScript_scrType] DEFAULT ((0)) NOT NULL,
    [scrVersion] BIGINT              CONSTRAINT [DF_dbScript_scrVersion] DEFAULT ((1)) NOT NULL,
    [scrAuthor]  NVARCHAR (50)       NULL,
    [scrText]    [dbo].[uXML]        NULL,
    [scrBlob]    VARBINARY (MAX)     NULL,
    [scrFormat]  SMALLINT            CONSTRAINT [DF_dbScript_scrFormat] DEFAULT ((0)) NOT NULL,
    [Created]    [dbo].[uCreated]    CONSTRAINT [DF_dbScript_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  [dbo].[uCreatedBy]  NULL,
    [Updated]    [dbo].[uCreated]    NULL,
    [UpdatedBy]  [dbo].[uCreatedBy]  NULL,
    [scrFlag]    INT                 CONSTRAINT [DF_dbScript_scrflag] DEFAULT ((0)) NOT NULL,
    [rowguid]    UNIQUEIDENTIFIER    CONSTRAINT [DF_dbScript_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbScript] PRIMARY KEY CLUSTERED ([scrCode] ASC, [scrType] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbScript_dbScriptType] FOREIGN KEY ([scrType]) REFERENCES [dbo].[dbScriptType] ([scrType]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbScript_rowguid]
    ON [dbo].[dbScript]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbScript] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbScript] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbScript] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbScript] TO [OMSApplicationRole]
    AS [dbo];

