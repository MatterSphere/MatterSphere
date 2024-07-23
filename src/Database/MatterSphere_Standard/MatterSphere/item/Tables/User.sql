CREATE TABLE [item].[User] (
    [ID]       UNIQUEIDENTIFIER CONSTRAINT [DF_User_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [NTLogin]  NVARCHAR (200)   NOT NULL,
    [Name]     NVARCHAR (50)    NOT NULL,
    [Active]   BIT              CONSTRAINT [DF_User_Active] DEFAULT ((1)) NOT NULL,
    [PolicyID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_User_NTLogin] UNIQUE NONCLUSTERED ([NTLogin] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[item].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[item].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[item].[User] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[item].[User] TO [OMSApplicationRole]
    AS [dbo];

