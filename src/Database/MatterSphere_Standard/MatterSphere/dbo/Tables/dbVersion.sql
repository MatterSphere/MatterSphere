CREATE TABLE [dbo].[dbVersion] (
    [verMajor]    TINYINT          NOT NULL,
    [verMinor]    TINYINT          NOT NULL,
    [verRevision] SMALLINT         NOT NULL,
    [verBuild]    INT              NOT NULL,
    [verBeta]     TINYINT          NULL,
    [verTag]      NVARCHAR (25)    NULL,
    [verNotes]    NVARCHAR (2000)  NULL,
    [verDate]     DATETIME         CONSTRAINT [DF_dbVersion_VerDate] DEFAULT (getutcdate()) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dbVersion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbVersion] PRIMARY KEY CLUSTERED ([verMajor] ASC, [verMinor] ASC, [verRevision] ASC, [verBuild] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbVersion_rowguid]
    ON [dbo].[dbVersion]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbVersion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbVersion] TO [OMSApplicationRole]
    AS [dbo];

