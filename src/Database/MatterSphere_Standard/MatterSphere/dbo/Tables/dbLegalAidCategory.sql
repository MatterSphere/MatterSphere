CREATE TABLE [dbo].[dbLegalAidCategory] (
    [LegAidCategory]        SMALLINT         NOT NULL,
    [LegAidDesc]            NVARCHAR (40)    NOT NULL,
    [LegAidCreditLimit]     MONEY            CONSTRAINT [DF_dbLegalAidCategory_LegAidCreditLimit] DEFAULT ((0)) NOT NULL,
    [LegAidFullCertificate] BIT              NULL,
    [LegAidActive]          BIT              CONSTRAINT [DF_dbLegalAidCategory_LegAidActive] DEFAULT ((1)) NOT NULL,
    [rowguid]               UNIQUEIDENTIFIER CONSTRAINT [DF_dbLegalAidCategory_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbLegalAidCategory] PRIMARY KEY CLUSTERED ([LegAidCategory] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbLegalAidCategory_rowguid]
    ON [dbo].[dbLegalAidCategory]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbLegalAidCategory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbLegalAidCategory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbLegalAidCategory] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbLegalAidCategory] TO [OMSApplicationRole]
    AS [dbo];

