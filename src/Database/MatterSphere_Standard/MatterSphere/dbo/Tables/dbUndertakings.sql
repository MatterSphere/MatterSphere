CREATE TABLE [dbo].[dbUndertakings] (
    [undID]            INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]           BIGINT              NOT NULL,
    [clID]             BIGINT              NULL,
    [undType]          [dbo].[uCodeLookup] NOT NULL,
    [undRef]           NVARCHAR (30)       NULL,
    [undDesc]          NVARCHAR (50)       NULL,
    [undAuthBy]        INT                 NULL,
    [undNotes]         NVARCHAR (MAX)      NULL,
    [undEstCompletion] DATETIME            NULL,
    [undCompleted]     DATETIME            NULL,
    [undCompletedBy]   NVARCHAR (12)       NULL,
    [undActive]        BIT                 CONSTRAINT [DF_dbUndertakings_undActive] DEFAULT ((1)) NOT NULL,
    [Created]          [dbo].[uCreated]    CONSTRAINT [DF_dbUndertakings_Created] DEFAULT (getutcdate()) NULL,
    [CreatedBy]        NVARCHAR (12)       NULL,
    [rowguid]          UNIQUEIDENTIFIER    CONSTRAINT [DF_dbUndertakings_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbUndertaking] PRIMARY KEY CLUSTERED ([undID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUndertakings_rowguid]
    ON [dbo].[dbUndertakings]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbUndertakings] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbUndertakings] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbUndertakings] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbUndertakings] TO [OMSApplicationRole]
    AS [dbo];

