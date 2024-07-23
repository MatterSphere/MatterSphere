CREATE TABLE [audit].[UserGroup_Contact] (
    [ID]          UNIQUEIDENTIFIER    CONSTRAINT [DF_UserGroup_Contact_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Created]     DATETIME            CONSTRAINT [DF_UserGroup_Contact_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   INT                 NOT NULL,
    [Event]       [dbo].[uCodeLookup] NOT NULL,
    [UserGroupID] UNIQUEIDENTIFIER    NOT NULL,
    [ContactID]   BIGINT              NOT NULL,
    [PolicyID]    UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_UserGroup_Contact] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[UserGroup_Contact] TO [OMSApplicationRole]
    AS [dbo];

