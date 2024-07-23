CREATE TABLE [dbo].[dbTeam] (
    [tmID]             INT                IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [tmCode]           NVARCHAR (15)      NOT NULL,
    [brID]             INT                NULL,
    [tmSpecialisation] NVARCHAR (MAX)     NULL,
    [tmLeader]         INT                NOT NULL,
    [Created]          [dbo].[uCreated]   NOT NULL,
    [CreatedBy]        [dbo].[uCreatedBy] NULL,
    [Updated]          [dbo].[uCreated]   NOT NULL,
    [UpdatedBy]        [dbo].[uCreatedBy] NULL,
    [rowguid]          UNIQUEIDENTIFIER   CONSTRAINT [DF_dbTeam_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_omsTeam] PRIMARY KEY CLUSTERED ([tmID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbTeam_dbBranch] FOREIGN KEY ([brID]) REFERENCES [dbo].[dbBranch] ([brID]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbTeam_Code] UNIQUE NONCLUSTERED ([tmCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbTeam_rowguid]
    ON [dbo].[dbTeam]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbTeam] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbTeam] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbTeam] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbTeam] TO [OMSApplicationRole]
    AS [dbo];

