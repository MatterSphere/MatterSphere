CREATE TABLE [dbo].[dbTerminal] (
    [termName]          NVARCHAR (50)    NOT NULL,
    [termADID]          NVARCHAR (50)    NULL,
    [termLoggedIn]      BIT              CONSTRAINT [DF_omsTerminal_termLoggedIn] DEFAULT ((0)) NOT NULL,
    [termLastUser]      INT              NULL,
    [termLastLogin]     DATETIME         NULL,
    [termCheckSum]      NVARCHAR (50)    NULL,
    [termRegisteredFor] NVARCHAR (1000)  NULL,
    [termVersion]       VARCHAR (50)     NULL,
    [termXML]           [dbo].[uXML]     CONSTRAINT [DF_dbTerminal_termXML] DEFAULT (N'<config/>') NOT NULL,
    [brID]              INT              NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_dbTerminal_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbTerminal] PRIMARY KEY CLUSTERED ([termName] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbTerminal_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTerminal_rowguid]
    ON [dbo].[dbTerminal]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTerminal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTerminal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTerminal] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTerminal] TO [OMSApplicationRole]
    AS [dbo];

