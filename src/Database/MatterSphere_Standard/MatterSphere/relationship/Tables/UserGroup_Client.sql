CREATE TABLE [relationship].[UserGroup_Client] (
    [RelationshipID]    UNIQUEIDENTIFIER CONSTRAINT [DF_UserGroup_Client_RelationshipID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserGroupID]       UNIQUEIDENTIFIER NOT NULL,
    [PolicyID]          UNIQUEIDENTIFIER NOT NULL,
    [ClientID]          BIGINT           NOT NULL,
    [block_inheritance] CHAR (1)         NULL,
    CONSTRAINT [PK_UserGroup_Client] PRIMARY KEY CLUSTERED ([RelationshipID] ASC),
    CONSTRAINT [FK_UserGroup_Client_ObjectPolicy_ID] FOREIGN KEY ([PolicyID]) REFERENCES [config].[ObjectPolicy] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [IX_UserGroup_Client] UNIQUE NONCLUSTERED ([ClientID] ASC, [UserGroupID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_Client_ClientId_UserGroupID]
    ON [relationship].[UserGroup_Client]([ClientID] ASC, [UserGroupID] ASC)
    INCLUDE([RelationshipID], [PolicyID]);


GO
GRANT UPDATE
    ON OBJECT::[relationship].[UserGroup_Client] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[relationship].[UserGroup_Client] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[relationship].[UserGroup_Client] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[relationship].[UserGroup_Client] TO [OMSApplicationRole]
    AS [dbo];

