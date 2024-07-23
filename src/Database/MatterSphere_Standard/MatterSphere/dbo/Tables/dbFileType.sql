CREATE TABLE [dbo].[dbFileType] (
    [typeCode]              [dbo].[uCodeLookup] NOT NULL,
    [typeVersion]           BIGINT              CONSTRAINT [DF_dbFileType_fileTypeVersion] DEFAULT ((0)) NOT NULL,
    [typeXML]               [dbo].[uXML]        CONSTRAINT [DF_dbFileType_typeXML] DEFAULT (N'<Config/>') NOT NULL,
    [typeGlyph]             INT                 CONSTRAINT [DF_dbFileType_typeGlyph] DEFAULT ((-1)) NOT NULL,
    [typeSeed]              [dbo].[uCodeLookup] NULL,
    [typeActive]            BIT                 CONSTRAINT [DF_dbFileType_fileActive] DEFAULT ((1)) NOT NULL,
    [fileDeptCode]          [dbo].[uCodeLookup] NOT NULL,
    [fileDefFundCode]       [dbo].[uCodeLookup] NOT NULL,
    [fileDestroyPeriod]     SMALLINT            CONSTRAINT [DF_dbFileType_fileDestroyPeriod] DEFAULT ((0)) NOT NULL,
    [filePrecLibrary]       [dbo].[uCodeLookup] NULL,
    [filePrecCategory]      [dbo].[uCodeLookup] NULL,
    [filePrecSubCategory]   [dbo].[uCodeLookup] NULL,
    [filePrecMinorCategory] [dbo].[uCodeLookup] NULL,
    [fileAccCode]           NVARCHAR (30)       NULL,
    [fileScript]            [dbo].[uCodeLookup] NULL,
    [fileField1Desc]        NVARCHAR (20)       NULL,
    [fileField2Desc]        NVARCHAR (20)       NULL,
    [fileField3Desc]        NVARCHAR (20)       NULL,
    [fileField4Desc]        NVARCHAR (20)       NULL,
    [fileField5Desc]        NVARCHAR (20)       NULL,
    [fileField6Desc]        NVARCHAR (20)       NULL,
    [fileField7Desc]        NVARCHAR (20)       NULL,
    [fileField8Desc]        NVARCHAR (20)       NULL,
    [fileField9Desc]        NVARCHAR (20)       NULL,
    [fileField10Desc]       NVARCHAR (20)       NULL,
    [fileFieldDate1Desc]    NVARCHAR (20)       NULL,
    [fileFieldDate2Desc]    NVARCHAR (20)       NULL,
    [fileFieldDate3Desc]    NVARCHAR (20)       NULL,
    [fileFieldDate4Desc]    NVARCHAR (20)       NULL,
    [fileFieldDate5Desc]    NVARCHAR (20)       NULL,
    [fileMilestoneActive]   BIT                 CONSTRAINT [DF_dbFileType_MatMilestoneActive] DEFAULT ((0)) NOT NULL,
    [fileMilestoneCode]     [dbo].[uCodeLookup] NULL,
    [fileContractGroup]     NVARCHAR (12)       NULL,
    [fileDefRiskCode]       [dbo].[uCodeLookup] NULL,
    [fileActionToDo]        NVARCHAR (150)      NULL,
    [fileLetterhead]        [dbo].[uFilePath]   NULL,
    [fileClientCareLetter]  [dbo].[uFilePath]   NULL,
    [fileTemplateDir]       [dbo].[uDirectory]  NULL,
    [fileConfLetter]        [dbo].[uFilePath]   NULL,
    [fileExpLetter]         [dbo].[uFilePath]   NULL,
    [fileFileSynopsis]      [dbo].[uFilePath]   NULL,
    [fileDefBankCode]       NVARCHAR (6)        NULL,
    [fileDefOffBankCode]    NVARCHAR (6)        NULL,
    [fileReview]            BIT                 CONSTRAINT [DF_dbFileType_fileReview] DEFAULT ((1)) NOT NULL,
    [fileReviewDays]        INT                 CONSTRAINT [DF_dbFileType_fileReviewDays] DEFAULT ((0)) NOT NULL,
    [fileSecurity]          BIT                 NULL,
    [fileRemoteAccSet]      NVARCHAR (10)       NULL,
    [rowguid]               UNIQUEIDENTIFIER    CONSTRAINT [DF_dbFileType_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [fileTimeActivityGroup] [dbo].[uCodeLookup] NULL,
    CONSTRAINT [PK_dbFileType] PRIMARY KEY CLUSTERED ([typeCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbFileType_rowguid]
    ON [dbo].[dbFileType]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbFileType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbFileType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbFileType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbFileType] TO [OMSApplicationRole]
    AS [dbo];

