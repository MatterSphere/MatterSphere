CREATE TABLE [audit].[User] (
    [ID]        UNIQUEIDENTIFIER    CONSTRAINT [DF_User_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Updated]   DATETIME            CONSTRAINT [DF_User_Created] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy] INT                 NOT NULL,
    [Event]     [dbo].[uCodeLookup] NOT NULL,
    [UserID]    UNIQUEIDENTIFIER    NOT NULL,
    [Name]      NVARCHAR (50)       NOT NULL,
    [NTLogin]   NVARCHAR (200)      NOT NULL,
    [Active]    BIT                 NOT NULL,
    [PolicyID]  UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[audit].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[audit].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[audit].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[audit].[User] TO [OMSApplicationRole]
    AS [dbo];

