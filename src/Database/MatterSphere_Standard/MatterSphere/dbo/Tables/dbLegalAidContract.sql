CREATE TABLE [dbo].[dbLegalAidContract] (
    [LAContractName]           NVARCHAR (30)    NULL,
    [LAContractCode]           NVARCHAR (12)    NOT NULL,
    [LAContractRef]            NVARCHAR (50)    NULL,
    [LAContractStarts]         INT              NULL,
    [LAContractTollerance]     INT              NULL,
    [LAContractStartDate]      DATETIME         NULL,
    [LAContractEndDate]        DATETIME         NULL,
    [LAContractMonths]         INT              NULL,
    [LAContractHoursPMonth]    INT              NULL,
    [LAContractTollExclusions] NVARCHAR (255)   NULL,
    [LAContractPrincipleMType] NVARCHAR (12)    NULL,
    [LAContractBranch]         NVARCHAR (12)    NULL,
    [rowguid]                  UNIQUEIDENTIFIER CONSTRAINT [DF_dbLegalAidContract_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbLegalAidContract] PRIMARY KEY CLUSTERED ([LAContractCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbLegalAidContract_rowguid]
    ON [dbo].[dbLegalAidContract]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbLegalAidContract] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbLegalAidContract] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbLegalAidContract] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbLegalAidContract] TO [OMSApplicationRole]
    AS [dbo];

