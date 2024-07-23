CREATE TABLE [dbo].[dbImages] (
    [ID]        BIGINT             IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Text]      NVARCHAR (50)      NULL,
    [Image]     VARBINARY (MAX)    NULL,
    [Created]   [dbo].[uCreated]   CONSTRAINT [DF_dbImages_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy] [dbo].[uCreatedBy] NULL,
    [Updated]   [dbo].[uCreated]   NULL,
    [Updatedby] [dbo].[uCreatedBy] NULL,
    [rowguid]   UNIQUEIDENTIFIER   CONSTRAINT [DF_dbImages_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbImages] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbImages_rowguid]
    ON [dbo].[dbImages]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbImages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbImages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbImages] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbImages] TO [OMSApplicationRole]
    AS [dbo];

