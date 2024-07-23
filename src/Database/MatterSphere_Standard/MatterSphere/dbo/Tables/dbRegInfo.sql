CREATE TABLE [dbo].[dbRegInfo] (
    [regCompanyName]                    NVARCHAR (50)          NOT NULL,
    [brID]                              INT                    NOT NULL,
    [regCompanyID]                      BIGINT                 NULL,
    [regEdition]                        CHAR (2)               NOT NULL,
    [regSerialNo]                       INT                    NOT NULL,
    [regSerialCheckSum]                 NVARCHAR (MAX)         NULL,
    [regSerialCheckKey]                 NVARCHAR (50)          NULL,
    [regAdministrator]                  INT                    NULL,
    [regTimeUnitValue]                  TINYINT                CONSTRAINT [DF_dbRegInfo_regTimeUnitValue] DEFAULT ((6)) NOT NULL,
    [regFailedLoginAttempts]            TINYINT                CONSTRAINT [DF_dbRegInfo_regFailedLoginAttempts] DEFAULT ((0)) NOT NULL,
    [regFinancialStart]                 SMALLDATETIME          NULL,
    [regSalesTaxRate]                   REAL                   CONSTRAINT [DF_dbRegInfo_regVATRate] DEFAULT ((17.5)) NOT NULL,
    [regTaskReminder]                   SMALLINT               CONSTRAINT [DF_dbRegInfo_regTaskReminder] DEFAULT ((14)) NOT NULL,
    [regInterestRate]                   REAL                   CONSTRAINT [DF_dbRegInfo_regInterestRate] DEFAULT ((0)) NOT NULL,
    [regMailEnabled]                    BIT                    CONSTRAINT [DF_dbRegInfo_regMailEnabled] DEFAULT ((0)) NOT NULL,
    [regMailServer]                     NVARCHAR (100)         NULL,
    [regConflictClientCheck]            BIT                    CONSTRAINT [DF_dbRegInfo_regConflictClientCheck] DEFAULT ((1)) NOT NULL,
    [regConflictFileCheck]              BIT                    CONSTRAINT [DF_dbRegInfo_regConflictMatterCheck] DEFAULT ((1)) NOT NULL,
    [regMarketNewClient]                BIT                    CONSTRAINT [DF_dbRegInfo_regMarketNewClient] DEFAULT ((0)) NOT NULL,
    [regSMSActive]                      BIT                    CONSTRAINT [DF_dbRegInfo_regSMSActive] DEFAULT ((0)) NOT NULL,
    [regPartnerWebSite]                 [dbo].[uURL]           NULL,
    [regPartnerSupportTel]              [dbo].[uTelephone]     NULL,
    [regPartnerSupportEmail]            [dbo].[uEmail]         NULL,
    [regPartnerTel]                     [dbo].[uTelephone]     NULL,
    [regPartnerFax]                     [dbo].[uTelephone]     NULL,
    [regPartnerAddress]                 BIGINT                 NULL,
    [regPartnerCompanyName]             NVARCHAR (50)          NULL,
    [regAddInstallInfo]                 NVARCHAR (200)         NULL,
    [regDefCountry]                     [dbo].[uCountry]       CONSTRAINT [DF_dbRegInfo_regDefCountry] DEFAULT ((223)) NOT NULL,
    [regcurISOCode]                     CHAR (3)               CONSTRAINT [DF_dbRegInfo_regcurISOCode] DEFAULT ('GBP') NOT NULL,
    [regUICultureInfo]                  [dbo].[uUICultureInfo] CONSTRAINT [DF_dbRegInfo_regUICultureInfo] DEFAULT ('en') NOT NULL,
    [regLoggingSeverity]                TINYINT                CONSTRAINT [DF_dbRegInfo_regLoggingSeverity] DEFAULT ((1)) NOT NULL,
    [regXML]                            [dbo].[uXML]           CONSTRAINT [DF_dbRegInfo_regXML] DEFAULT (N'<config/>') NOT NULL,
    [regSignature]                      VARBINARY (MAX)        NULL,
    [regDraftLogo]                      VARBINARY (MAX)        NULL,
    [regFileCopyLogo]                   VARBINARY (MAX)        NULL,
    [regDuplicateDocID]                 BIT                    CONSTRAINT [DF_dbRegInfo_regDuplicateDocID] DEFAULT ((0)) NOT NULL,
    [regDisclaimer]                     NVARCHAR (MAX)         NULL,
    [regSMTPServer]                     NVARCHAR (50)          NULL,
    [regDefDocStorageLoc]               SMALLINT               NULL,
    [regDefPrecStorageLoc]              SMALLINT               NULL,
    [regDocRetensionDays]               SMALLINT               CONSTRAINT [DF_dbRegInfo_regDocRetensionDays] DEFAULT ((30)) NOT NULL,
    [regBinSerialCheckSum]              VARBINARY (MAX)        NULL,
    [regBranchConfig]                   INT                    CONSTRAINT [DF_dbRegInfo_regBranchConfig] DEFAULT ((0)) NOT NULL,
    [regExternalDocumentPropertyPrefix] NVARCHAR (10)          NULL,
    [regDataKey]                        NVARCHAR (10)          NULL,
    [regExternalDocumentPropertyNames]  NVARCHAR (200)         NULL,
    [regBlockInheritence]               BIT                    CONSTRAINT [DF_dbRegInfo_regBlockInheritence] DEFAULT ((0)) NOT NULL,
    [regLocalSearchserverGroupsActive]  BIT                    CONSTRAINT [DF_dbRegInfo_regLocalSearchserverGroupsActive] DEFAULT ((0)) NULL,
    [regDefaultSystemLicense]           NVARCHAR (50)          NULL,
    [regExternalDocumentPropertyName]   NVARCHAR (200)         NULL,
    CONSTRAINT [PK_dbRegInfo] PRIMARY KEY CLUSTERED ([brID] ASC) WITH (FILLFACTOR = 90)
);




GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbRegInfo] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbRegInfo] TO [OMSApplicationRole]
    AS [dbo];

