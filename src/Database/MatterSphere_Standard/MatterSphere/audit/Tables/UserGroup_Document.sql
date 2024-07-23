CREATE TABLE [audit].[UserGroup_Document] (
    [ID]          UNIQUEIDENTIFIER    CONSTRAINT [DF_UserGroup_Document_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Created]     DATETIME            CONSTRAINT [DF_UserGroup_Document_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]   INT                 NOT NULL,
    [Event]       [dbo].[uCodeLookup] NOT NULL,
    [UserGroupID] UNIQUEIDENTIFIER    NOT NULL,
    [DocumentID]  BIGINT              NOT NULL,
    [PolicyID]    UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_UserGroup_Document] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[UserGroup_Document] TO [OMSApplicationRole]
    AS [dbo];

