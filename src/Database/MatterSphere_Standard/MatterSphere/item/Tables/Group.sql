CREATE TABLE [item].[Group] (
    [ID]                  UNIQUEIDENTIFIER    CONSTRAINT [DF_Group_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]                [dbo].[uCodeLookup] NOT NULL,
    [Description]         NVARCHAR (200)      NOT NULL,
    [Active]              BIT                 CONSTRAINT [DF_Group_Active] DEFAULT ((1)) NOT NULL,
    [PolicyID]            UNIQUEIDENTIFIER    NOT NULL,
    [ADDistinguishedName] NVARCHAR (200)      NOT NULL,
    [ADGUID]              UNIQUEIDENTIFIER    NULL,
    CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Group_Name] UNIQUE NONCLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[item].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[item].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[item].[Group] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[item].[Group] TO [OMSApplicationRole]
    AS [dbo];

