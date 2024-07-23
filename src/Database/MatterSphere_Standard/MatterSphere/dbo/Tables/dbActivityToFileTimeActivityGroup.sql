CREATE TABLE [dbo].[dbActivityToFileTimeActivityGroup] (
    [ID]                    BIGINT              IDENTITY (1, 1) NOT NULL,
    [actCode]               [dbo].[uCodeLookup] NOT NULL,
    [fileTimeActivityGroup] [dbo].[uCodeLookup] NOT NULL,
    [Active]                BIT                 CONSTRAINT [DF_dbActivityToFileTimeActivityGroup_Active] DEFAULT ((1)) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbActivityToFileTimeActivityGroup_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbActivityToFileTimeActivityGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_actCode_fileTimeActivityGroup] UNIQUE NONCLUSTERED ([actCode] ASC, [fileTimeActivityGroup] ASC)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbActivityToFileTimeActivityGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbActivityToFileTimeActivityGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbActivityToFileTimeActivityGroup] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbActivityToFileTimeActivityGroup] TO [OMSApplicationRole]
    AS [dbo];

