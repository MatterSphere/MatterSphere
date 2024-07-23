CREATE TABLE [dbo].[dbKeyDates] (
    [kdID]        INT                 IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [fileID]      BIGINT              NOT NULL,
    [kdRelatedID] UNIQUEIDENTIFIER    NOT NULL,
    [kdType]      [dbo].[uCodeLookup] NOT NULL,
    [kdDesc]      NVARCHAR (300)      NOT NULL,
    [kdDate]      DATETIME            NOT NULL,
    [kdActive]    BIT                 CONSTRAINT [DF_dbKeyDates_kdActive] DEFAULT ((1)) NOT NULL,
    [Created]     [dbo].[uCreated]    NULL,
    [CreatedBy]   [dbo].[uCreatedBy]  NULL,
    [rowguid]     UNIQUEIDENTIFIER    CONSTRAINT [DF_dbKeyDates_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbKeyDates] PRIMARY KEY CLUSTERED ([kdID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbKeyDates_dbDateWizTypes] FOREIGN KEY ([kdType]) REFERENCES [dbo].[dbDateWizTypes] ([typeCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbKeyDates_dbFile1] FOREIGN KEY ([fileID]) REFERENCES [dbo].[dbFile] ([fileID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbKeyDates_rowguid]
    ON [dbo].[dbKeyDates]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbKeyDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbKeyDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbKeyDates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbKeyDates] TO [OMSApplicationRole]
    AS [dbo];

