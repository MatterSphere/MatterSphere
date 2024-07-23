CREATE TABLE [dbo].[dbBranch] (
    [brID]                       INT                    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [brCode]                     NVARCHAR (16)          NOT NULL,
    [brName]                     NVARCHAR (50)          NOT NULL,
    [brDescription]              NVARCHAR (50)          NULL,
    [braddID]                    BIGINT                 NOT NULL,
    [br1LineAdd]                 NVARCHAR (100)         NULL,
    [brTelephone]                [dbo].[uTelephone]     NULL,
    [brFax]                      [dbo].[uTelephone]     NULL,
    [brVATNo]                    NVARCHAR (50)          NULL,
    [brWebsite]                  [dbo].[uURL]           NULL,
    [brIntranet]                 [dbo].[uURL]           NULL,
    [brHeadOffice]               BIT                    CONSTRAINT [DF_omsBranch_brHeadOffice] DEFAULT ((0)) NOT NULL,
    [brManager]                  INT                    NULL,
    [brUICultureInfo]            [dbo].[uUICultureInfo] NULL,
    [brDefCountry]               [dbo].[uCountry]       NULL,
    [brcurISOCode]               CHAR (3)               NULL,
    [brLegalAidRef]              NVARCHAR (12)          NULL,
    [brSignature]                VARBINARY (MAX)        NULL,
    [brIntActive]                BIT                    NULL,
    [brIntDefaultSecurity]       NVARCHAR (10)          NULL,
    [brIntEmailChangePwdBody]    NVARCHAR (MAX)         NULL,
    [brIntEmailChangePwdSubject] NVARCHAR (50)          NULL,
    [brIntEmailChangePwdFrom]    [dbo].[uEmail]         NULL,
    [brXML]                      [dbo].[uXML]           CONSTRAINT [DF_dbBranch_brXML] DEFAULT (N'<config/>') NOT NULL,
    [rowguid]                    UNIQUEIDENTIFIER       CONSTRAINT [DF_dbBranch_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbBranch] PRIMARY KEY CLUSTERED ([brID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_dbBranch_dbAddress] FOREIGN KEY ([braddID]) REFERENCES [dbo].[dbAddress] ([addID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbBranch_dbCountry] FOREIGN KEY ([brDefCountry]) REFERENCES [dbo].[dbCountry] ([ctryID]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbBranch_dbCurrency] FOREIGN KEY ([brcurISOCode]) REFERENCES [dbo].[dbCurrency] ([curISOCode]) NOT FOR REPLICATION,
    CONSTRAINT [FK_dbBranch_dbLanguage] FOREIGN KEY ([brUICultureInfo]) REFERENCES [dbo].[dbLanguage] ([langCode]) NOT FOR REPLICATION,
    CONSTRAINT [IX_dbBranch_Code] UNIQUE NONCLUSTERED ([brCode] ASC) WITH (FILLFACTOR = 90) ON [IndexGroup]
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbBranch_rowguid]
    ON [dbo].[dbBranch]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbBranch] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbBranch] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbBranch] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbBranch] TO [OMSApplicationRole]
    AS [dbo];

