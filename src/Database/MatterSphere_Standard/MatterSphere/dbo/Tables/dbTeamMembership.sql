CREATE TABLE [dbo].[dbTeamMembership] (
    [tmID]    INT              NOT NULL,
    [usrID]   INT              NOT NULL,
    [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbTeamMembership_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbTeamMembership] PRIMARY KEY CLUSTERED ([tmID] ASC, [usrID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbTeamMembership_dbTeam] FOREIGN KEY ([tmID]) REFERENCES [dbo].[dbTeam] ([tmID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbTeamMembership_dbUser] FOREIGN KEY ([usrID]) REFERENCES [dbo].[dbUser] ([usrID]) NOT FOR REPLICATION
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTeamMembership_rowguid]
    ON [dbo].[dbTeamMembership]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTeamMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTeamMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTeamMembership] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTeamMembership] TO [OMSApplicationRole]
    AS [dbo];

