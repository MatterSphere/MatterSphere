CREATE TABLE [audit].[Group_User] (
    [ID]        UNIQUEIDENTIFIER    CONSTRAINT [DF_Group_User_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Created]   DATETIME            CONSTRAINT [DF_Group_User_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] INT                 NOT NULL,
    [Event]     [dbo].[uCodeLookup] NOT NULL,
    [GroupID]   UNIQUEIDENTIFIER    NOT NULL,
    [UserID]    UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_Group_User] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[Group_User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[Group_User] TO [OMSApplicationRole]
    AS [dbo];

