CREATE TABLE [dbo].[dbEnquiryQuestion] (
    [quID]               BIGINT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [enqID]              INT                 NULL,
    [quName]             NVARCHAR (25)       NOT NULL,
    [quOrder]            TINYINT             CONSTRAINT [DF_dbEnquiryQuestion_quOrder] DEFAULT ((0)) NOT NULL,
    [quCode]             [dbo].[uCodeLookup] NOT NULL,
    [quPage]             [dbo].[uCodeLookup] NULL,
    [quTable]            VARCHAR (50)        NULL,
    [quExtendedData]     [dbo].[uCodeLookup] NULL,
    [quFieldName]        VARCHAR (50)        NULL,
    [quProperty]         VARCHAR (50)        NULL,
    [quType]             VARCHAR (50)        CONSTRAINT [DF_dbEnquiryQuestion_quType] DEFAULT ('System.String') NOT NULL,
    [quctrlid]           INT                 CONSTRAINT [DF_dbEnquiryQuestion_quControl] DEFAULT ((0)) NOT NULL,
    [quDataList]         [dbo].[uCodeLookup] NULL,
    [quAdd]              BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quAdd] DEFAULT ((1)) NOT NULL,
    [quEdit]             BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quEdit] DEFAULT ((1)) NOT NULL,
    [quAddSecLevel]      TINYINT             CONSTRAINT [DF_dbEnquiryQuestion_quSecurity] DEFAULT ((0)) NOT NULL,
    [quEditSecLevel]     TINYINT             CONSTRAINT [DF_dbEnquiryQuestion_quEditSecLevel] DEFAULT ((0)) NOT NULL,
    [quSearch]           BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quSearch] DEFAULT ((1)) NOT NULL,
    [quUnique]           BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quUnique] DEFAULT ((0)) NOT NULL,
    [quConstraint]       VARCHAR (75)        NULL,
    [quHidden]           BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quHidden] DEFAULT ((0)) NOT NULL,
    [quDefault]          NVARCHAR (255)      NULL,
    [quTabOrder]         INT                 CONSTRAINT [DF_dbEnquiryQuestion_quTabOrder] DEFAULT ((0)) NULL,
    [quReadonly]         BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quReadonly] DEFAULT ((0)) NOT NULL,
    [quRequired]         BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quRequired] DEFAULT ((0)) NOT NULL,
    [quMinLength]        INT                 CONSTRAINT [DF_dbEnquiryQuestion_quMinLength] DEFAULT ((0)) NOT NULL,
    [quMaxLength]        INT                 CONSTRAINT [DF_dbEnquiryQuestion_quMaxLength] DEFAULT ((255)) NOT NULL,
    [quWidth]            INT                 CONSTRAINT [DF_dbEnquiryQuestion_quDisplayLength] DEFAULT ((300)) NOT NULL,
    [quHeight]           INT                 CONSTRAINT [DF_dbEnquiryQuestion_quHeight] DEFAULT ((22)) NOT NULL,
    [quMask]             NVARCHAR (100)      NULL,
    [quX]                INT                 CONSTRAINT [DF_dbEnquiryQuestion_quX] DEFAULT ((0)) NOT NULL,
    [quY]                INT                 CONSTRAINT [DF_dbEnquiryQuestion_quY] DEFAULT ((0)) NOT NULL,
    [quWizX]             INT                 NULL,
    [quWizY]             INT                 NULL,
    [quCaptionWidth]     INT                 CONSTRAINT [DF_dbEnquiryQuestion_quCaptionWidth] DEFAULT ((150)) NOT NULL,
    [quCasing]           VARCHAR (10)        CONSTRAINT [DF_dbEnquiryQuestion_quCasing_1] DEFAULT ('Normal') NULL,
    [quColumn]           TINYINT             CONSTRAINT [DF_dbEnquiryQuestion_quColumn] DEFAULT ((0)) NOT NULL,
    [quEdition]          VARCHAR (2)         NULL,
    [quCommand]          [dbo].[uCodeLookup] NULL,
    [quCommandRetVal]    BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quCommandRetVal] DEFAULT ((1)) NOT NULL,
    [quSystem]           BIT                 CONSTRAINT [DF_dbEnquiryQuestion_quSystem] DEFAULT ((0)) NOT NULL,
    [quCustom]           VARBINARY (MAX)     NULL,
    [quFilter]           [dbo].[uXML]        CONSTRAINT [DF_dbEnquiryQuestion_quFilter] DEFAULT (N'<filters/>') NOT NULL,
    [quAnchor]           VARCHAR (10)        NULL,
    [quFormat]           VARCHAR (10)        NULL,
    [quHelpKeyword]      NVARCHAR (200)      NULL,
    [quCondition]        NVARCHAR (500)      NULL,
    [quRole]             NVARCHAR (200)      NULL,
    [quLicense]          NVARCHAR (200)      NULL,
    [quValidationAction] INT                 CONSTRAINT [DF_dbEnquiryQuestion_quValidationAction] DEFAULT ((0)) NOT NULL,
    [quVisibleRole]      NVARCHAR (200)      NULL,
    [quEditableRole]     NVARCHAR (200)      NULL,
    [rowguid]            UNIQUEIDENTIFIER    CONSTRAINT [DF_dbEnquiryQuestion_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbEnquiryQuestion] PRIMARY KEY CLUSTERED ([quID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbEnquiryQuestion_dbEnquiry] FOREIGN KEY ([enqID]) REFERENCES [dbo].[dbEnquiry] ([enqID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbEnquiryQuestion_dbEnquiryCommand] FOREIGN KEY ([quCommand]) REFERENCES [dbo].[dbEnquiryCommand] ([cmdCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbEnquiryQuestion_dbEnquiryControl] FOREIGN KEY ([quctrlid]) REFERENCES [dbo].[dbEnquiryControl] ([ctrlID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbEnquiryQuestion_dbEnquiryDataLists] FOREIGN KEY ([quDataList]) REFERENCES [dbo].[dbEnquiryDataList] ([enqTable]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbEnquiryQuestion_dbExtendedData] FOREIGN KEY ([quExtendedData]) REFERENCES [dbo].[dbExtendedData] ([extCode]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbEnquiryQuestion] UNIQUE NONCLUSTERED ([enqID] ASC, [quName] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE NONCLUSTERED INDEX [IX_dbEnquiryQuestion_quCode]
    ON [dbo].[dbEnquiryQuestion]([quCode] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbEnquiryQuestion_rowguid]
    ON [dbo].[dbEnquiryQuestion]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbEnquiryQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbEnquiryQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbEnquiryQuestion] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbEnquiryQuestion] TO [OMSApplicationRole]
    AS [dbo];

