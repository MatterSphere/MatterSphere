CREATE TABLE [dbo].[dbCDSRates] (
    [CDSCategory]  NVARCHAR (50)    NOT NULL,
    [CDSDesc]      NVARCHAR (50)    NULL,
    [CDSLSFHLimit] MONEY            NULL,
    [CDSLSFCharge] MONEY            NULL,
    [CDSHSFHLimit] MONEY            NULL,
    [CDSHSFCharge] MONEY            NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_dbCDSRates_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCDSRates] PRIMARY KEY CLUSTERED ([CDSCategory] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCDSRates_rowguid]
    ON [dbo].[dbCDSRates]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCDSRates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCDSRates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCDSRates] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCDSRates] TO [OMSApplicationRole]
    AS [dbo];

