CREATE TABLE [config].[ObjectPolicy] (
    [ID]        UNIQUEIDENTIFIER    CONSTRAINT [DF_ObjectPolicy_ID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Type]      [dbo].[uCodeLookup] NOT NULL,
    [AllowMask] VARBINARY (32)      NULL,
    [DenyMask]  VARBINARY (32)      NULL,
    [Name]      NVARCHAR (100)      NULL,
    [IsRemote]  BIT                 NULL,
    CONSTRAINT [PK_ObjectPolicy] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[config].[ObjectPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[ObjectPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[ObjectPolicy] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[ObjectPolicy] TO [OMSApplicationRole]
    AS [dbo];

