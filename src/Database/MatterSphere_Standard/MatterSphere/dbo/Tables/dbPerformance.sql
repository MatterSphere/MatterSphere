CREATE TABLE [dbo].[dbPerformance] (
    [perfID]      INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [contID]      BIGINT           NOT NULL,
    [perfJobDesc] NVARCHAR (50)    NOT NULL,
    [perfNumber]  TINYINT          NOT NULL,
    [perfDate]    DATETIME         NULL,
    [perfNotepad] NVARCHAR (MAX)   NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbPerformance_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbPerformance] PRIMARY KEY CLUSTERED ([perfID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbPerformance_dbContact] FOREIGN KEY ([contID]) REFERENCES [dbo].[dbContact] ([contID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbPerformance_rowguid]
    ON [dbo].[dbPerformance]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbPerformance] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbPerformance] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbPerformance] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbPerformance] TO [OMSApplicationRole]
    AS [dbo];

