CREATE TABLE [dbo].[dbEnquiry] (
    [enqID]                INT                 NOT NULL,
    [enqCode]              [dbo].[uCodeLookup] NOT NULL,
    [enqVersion]           BIGINT              CONSTRAINT [DF_dbEnquiry_enqVersion] DEFAULT ((0)) NOT NULL,
    [enqEngineVersion]     BIGINT              CONSTRAINT [DF_dbEnquiry_enqEngineVersion] DEFAULT ((0)) NOT NULL,
    [enqSourceType]        [dbo].[uCodeLookup] CONSTRAINT [DF_dbEnquiry_enqSourceType] DEFAULT (N'OMS') NOT NULL,
    [enqSource]            NVARCHAR (255)      NULL,
    [enqCall]              NVARCHAR (100)      NULL,
    [enqFields]            NVARCHAR (MAX)      NULL,
    [enqWhere]             NVARCHAR (500)      NULL,
    [enqParameters]        [dbo].[uXML]        CONSTRAINT [DF_dbEnquiry_enqParameters] DEFAULT (N'<params xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" />') NULL,
    [enqWelcomeHeaderCode] [dbo].[uCodeLookup] NULL,
    [enqWelcomeTextCode]   [dbo].[uCodeLookup] NULL,
    [enqPaddingX]          INT                 CONSTRAINT [DF_dbEnquiry_enqLeadingX] DEFAULT ((0)) NOT NULL,
    [enqPaddingY]          INT                 CONSTRAINT [DF_dbEnquiry_enqPaddingY] DEFAULT ((0)) NOT NULL,
    [enqLeadingX]          INT                 CONSTRAINT [DF_dbEnquiry_enqLeadingX_1] DEFAULT ((0)) NOT NULL,
    [enqLeadingY]          INT                 CONSTRAINT [DF_dbEnquiry_enqLeadingY] DEFAULT ((0)) NOT NULL,
    [enqModes]             TINYINT             CONSTRAINT [DF_dbEnquiry_enqModes] DEFAULT ((0)) NOT NULL,
    [enqBindings]          TINYINT             CONSTRAINT [DF_dbEnquiry_enqBinding] DEFAULT ((0)) NOT NULL,
    [enqCanvasHeight]      INT                 CONSTRAINT [DF_dbEnquiry_enqCanvasHeight] DEFAULT ((394)) NOT NULL,
    [enqCanvasWidth]       INT                 CONSTRAINT [DF_dbEnquiry_enqCanvasWidth] DEFAULT ((506)) NOT NULL,
    [enqWizardHeight]      INT                 CONSTRAINT [DF_dbEnquiry_enqWizardHeight] DEFAULT ((394)) NOT NULL,
    [enqWizardWidth]       INT                 CONSTRAINT [DF_dbEnquiry_enqWizardWidth] DEFAULT ((506)) NOT NULL,
    [enqSystem]            BIT                 CONSTRAINT [DF_dbEnquiry_enqSystem] DEFAULT ((0)) NOT NULL,
    [enqPassword]          NVARCHAR (15)       NULL,
    [enqDesManBindMode]    BIT                 CONSTRAINT [DF_dbEnquiry_enqDesManBindMode] DEFAULT ((0)) NOT NULL,
    [enqPath]              [dbo].[uFilePath]   CONSTRAINT [DF_dbEnquiry_enqPath] DEFAULT (N'\') NOT NULL,
    [enqImage]             VARBINARY (MAX)     NULL,
    [enqSettings]          [dbo].[uXML]        NULL,
    [enqScript]            [dbo].[uCodeLookup] NULL,
    [enqHelp]              NVARCHAR (150)      NULL,
    [enqHelpKeyword]       NVARCHAR (200)      NULL,
    [Created]              [dbo].[uCreated]    CONSTRAINT [DF_dbEnquiry_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]            [dbo].[uCreatedBy]  NULL,
    [Updated]              [dbo].[uCreated]    CONSTRAINT [DF_dbEnquiry_Updated] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]            [dbo].[uCreatedBy]  NULL,
    [rowguid]              UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiry_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnquiry] PRIMARY KEY CLUSTERED ([enqID] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiry_enqCode]
    ON [dbo].[dbEnquiry]([enqCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiry_rowguid]
    ON [dbo].[dbEnquiry]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiry] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiry] TO [OMSApplicationRole]
    AS [dbo];

