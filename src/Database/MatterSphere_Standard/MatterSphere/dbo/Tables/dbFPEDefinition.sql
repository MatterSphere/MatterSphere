CREATE TABLE [dbo].[dbFPEDefinition] (
    [ID]         INT              NOT NULL,
    [Name]       NVARCHAR (50)    NULL,
    [Definition] NVARCHAR (MAX)   NULL,
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbFPEDefinition_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbFPEDefination] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFPEDefinition_rowguid]
    ON [dbo].[dbFPEDefinition]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFPEDefinition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFPEDefinition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFPEDefinition] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFPEDefinition] TO [OMSApplicationRole]
    AS [dbo];

