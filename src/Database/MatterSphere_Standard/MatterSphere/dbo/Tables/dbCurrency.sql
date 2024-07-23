CREATE TABLE [dbo].[dbCurrency] (
    [curISOCode]          CHAR (3)         NOT NULL,
    [curName]             NVARCHAR (100)   NOT NULL,
    [curSign]             NVARCHAR (10)    NOT NULL,
    [curSignDesc]         NVARCHAR (50)    NULL,
    [curRate]             REAL             CONSTRAINT [DF_dbCurrency_curRate] DEFAULT ((1)) NOT NULL,
    [curDecimalPlaces]    TINYINT          CONSTRAINT [DF_dbCurrency_curDecimalPlaces_1] DEFAULT ((2)) NOT NULL,
    [curDecimalSeperator] NVARCHAR (1)     CONSTRAINT [DF_dbCurrency_curDecimalSeperator_1] DEFAULT (N'.') NOT NULL,
    [curGroupSeparator]   NVARCHAR (1)     CONSTRAINT [DF_dbCurrency_curGroupSeparator_1] DEFAULT (N',') NOT NULL,
    [curNegativePattern]  TINYINT          CONSTRAINT [DF_dbCurrency_curNegativePattern_1] DEFAULT ((1)) NOT NULL,
    [curPositivePattern]  TINYINT          CONSTRAINT [DF_dbCurrency_curPositivePattern_1] DEFAULT ((0)) NOT NULL,
    [curActive]           BIT              CONSTRAINT [DF_dbCurrency_curActive] DEFAULT ((1)) NOT NULL,
    [rowguid]             UNIQUEIDENTIFIER CONSTRAINT [DF_dbCurrency_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dbCurrency] PRIMARY KEY CLUSTERED ([curISOCode] ASC) WITH (FILLFACTOR = 90)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dbCurrency_rowguid]
    ON [dbo].[dbCurrency]([rowguid] ASC) WITH (FILLFACTOR = 90)
    ON [IndexGroup];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[dbCurrency] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[dbCurrency] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[dbCurrency] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[dbCurrency] TO [OMSApplicationRole]
    AS [dbo];

