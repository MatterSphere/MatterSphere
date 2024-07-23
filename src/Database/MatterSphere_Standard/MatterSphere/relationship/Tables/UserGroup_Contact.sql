CREATE TABLE [relationship].[UserGroup_Contact] (
    [RelationshipID] UNIQUEIDENTIFIER CONSTRAINT [DF_UserGroup_Contact_RelationshipID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserGroupID]    UNIQUEIDENTIFIER NOT NULL,
    [ContactID]      BIGINT           NOT NULL,
    [PolicyID]       UNIQUEIDENTIFIER NOT NULL,
    [clid]           BIGINT           DEFAULT ((0)) NULL,
    [inherited]      CHAR (1)         NULL,
    CONSTRAINT [PK_UserGroup_Contact] PRIMARY KEY CLUSTERED ([RelationshipID] ASC),
    CONSTRAINT [IX_UserGroup_Contact] UNIQUE NONCLUSTERED ([ContactID] ASC, [UserGroupID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_UserGroup_Contact_ContactID_UserGroupID]
    ON [relationship].[UserGroup_Contact]([ContactID] ASC, [UserGroupID] ASC)
    INCLUDE([RelationshipID], [PolicyID]);


GO
GRANT UPDATE
    ON OBJECT::[relationship].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[relationship].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[relationship].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[relationship].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];

