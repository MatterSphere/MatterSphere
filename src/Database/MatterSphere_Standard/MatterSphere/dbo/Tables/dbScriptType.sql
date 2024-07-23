CREATE TABLE [dbo].[dbScriptType] (
    [scrType]         [dbo].[uCodeLookup] NOT NULL,
    [scrAssemblyType] NVARCHAR (100)      CONSTRAINT [DF_dbScriptType_scrXml] DEFAULT (N'<script/>') NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER    CONSTRAINT [DF_dbScriptType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbScriptType] PRIMARY KEY CLUSTERED ([scrType] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbScriptType_rowguid]
    ON [dbo].[dbScriptType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbScriptType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbScriptType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbScriptType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbScriptType] TO [OMSApplicationRole]
    AS [dbo];

