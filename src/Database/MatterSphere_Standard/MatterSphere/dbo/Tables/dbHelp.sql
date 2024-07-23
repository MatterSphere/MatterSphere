CREATE TABLE [dbo].[dbHelp] (
    [helpID]  INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]   NVARCHAR (150)   NULL,
    [RTF]     NVARCHAR (MAX)   NULL,
    [RAW]     NVARCHAR (MAX)   NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbHelp_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbHelp] PRIMARY KEY CLUSTERED ([helpID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbHelp_rowguid]
    ON [dbo].[dbHelp]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbHelp] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbHelp] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbHelp] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbHelp] TO [OMSApplicationRole]
    AS [dbo];

