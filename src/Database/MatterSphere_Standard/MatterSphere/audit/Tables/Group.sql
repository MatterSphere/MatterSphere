CREATE TABLE [audit].[Group] (
    [ID]          UNIQUEIDENTIFIER    CONSTRAINT [DF_Group_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Updated]     DATETIME            CONSTRAINT [DF_Group_Created] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]   INT                 NOT NULL,
    [Event]       [dbo].[uCodeLookup] NOT NULL,
    [GroupID]     UNIQUEIDENTIFIER    NOT NULL,
    [Name]        NVARCHAR (50)       NOT NULL,
    [Description] NVARCHAR (200)      NOT NULL,
    [Active]      BIT                 NOT NULL,
    [PolicyID]    UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[Group] TO [OMSApplicationRole]
    AS [dbo];

